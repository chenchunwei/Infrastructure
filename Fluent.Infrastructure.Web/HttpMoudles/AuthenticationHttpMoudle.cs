using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Infrastructure.Log;
using Fluent.Infrastructure.ServiceLocation;
using Fluent.Infrastructure.Utilities;
using Fluent.Infrastructure.Web.HttpMoudles.Configuration;
using log4net;


namespace Fluent.Infrastructure.Web.HttpMoudles
{
    public class AuthenticationHttpMoudle : IHttpModule
    {
        private readonly ILog _logger;

        private readonly IUserService _userService;
        private readonly DesCrypto _desCrypto;

        public AuthenticationHttpMoudle()
        {
            _logger = new DefaultLoggerFactory().GetLogger();
            _logger.DebugFormat("AuthenticationHttpMoudle创建新的实例");
            _userService = ServiceLocationHandler.Resolver<IUserService>();
            _desCrypto = new DesCrypto("hhyjuuhd", "mmnjikjh");
        }
        public void Dispose()
        {
            return;
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Handler;
        }

        public void Handler(object sender, EventArgs args)
        {
            _logger.DebugFormat("AuthenticationHttpMoudle.Handler被访问，对象实例为{0}", this.GetHashCode());
            var httpContext = HttpContext.Current;
            if (httpContext == null)
                throw new ApplicationException("context");
            if (httpContext.Request == null)
                throw new NullReferenceException("httpContext.Request");
            if (httpContext.Request.Cookies == null)
                throw new NullReferenceException("httpContext.Request.Cookies");

            //判断是起登录请求
            string loginParam = httpContext.Request.QueryString.Get(LoginParam);
            if (!string.IsNullOrEmpty(loginParam) && loginParam.Equals(LoginValue, StringComparison.CurrentCultureIgnoreCase))
            {
                Login();
            }
            //判断是否发起登出请求
            var logoutParameter = httpContext.Request.QueryString.Get(LogoutParam);
            if (!string.IsNullOrEmpty(logoutParameter) && logoutParameter.Equals(LogoutValue, StringComparison.CurrentCultureIgnoreCase))
            {
                Logout();
            }
            //_logger.InfoFormat("访问页面的绝对地址:" + httpContext.Request.Url.AbsolutePath);
            //判断是否在忽略列表中
            var accountCookie = httpContext.Request.Cookies[LoginCookieKey];
            var extensionCookie = httpContext.Request.Cookies[LoginExtensionCookieKey];
            if (accountCookie == null)
            {
                if (IsInIgronSettings(httpContext.Request))
                    return;
                if (!httpContext.Request.Url.AbsolutePath.Equals(ConvertUrl(LoginUrl), StringComparison.CurrentCultureIgnoreCase))
                    RedirectToLoginUrl();
            }
            else
            {
                //HACK:问题，当用户已登录的时候再登入的时候，会多次加载用户信息。这个需要修正，而且比较麻烦
                //多次加载的问题是因为样式、图片、js均会发起请求并进入Moudle进行处理
                LoadUserProfie(accountCookie, extensionCookie);
            }
        }

        private bool IsInIgronSettings(HttpRequest request)
        {
            _logger.DebugFormat("正校验地址{0}是否在忽略列表中", request.Url.AbsolutePath);
            var section = ConfigurationManager.GetSection("AuthencationSection") as AuthencationSection;
            if (section == null)
                throw new ConfigurationErrorsException("web.config中必须配置AuthencationSection节点信息");
            var isIngronePath = section.IgnorePaths.Cast<IgnorePathConfigurationElement>()
                .Any(igrone => request.Url.AbsolutePath.StartsWith(ConvertUrl(igrone.Path), StringComparison.CurrentCultureIgnoreCase));
            if (!isIngronePath)
                return section.IgnorePostfixs.Cast<IgnorePostfixConfigurationElement>()
                   .Any(igronePostfix =>
                       request.FilePath
                       .EndsWith(igronePostfix.Postfix, StringComparison.CurrentCultureIgnoreCase));
            _logger.DebugFormat("地址{0}存在于忽略列表中", request.Url.AbsolutePath);
            return true;
        }

        private void LoadUserProfie(HttpCookie accountCookie, HttpCookie extensionCookie)
        {
            var httpContext = HttpContext.Current;
            var accountName = _desCrypto.Decryptor(accountCookie.Value);
            var user = _userService.GetAuthencationUser(accountName, extensionCookie == null ? "" : extensionCookie.Value);
            if (user != null)
            {
                httpContext.Items[HttpMoudlesConst.HttpUserKey] = user;
                _logger.DebugFormat("用户信息正常加载，用户名：{0},请求地址：{1}", accountName, httpContext.Request.Url.AbsolutePath);
            }
            else
            {
                _logger.DebugFormat("用户加载失败，将进行跳转，用户名：{0}", accountName);
                RemoveLoginCookie(httpContext, accountCookie, extensionCookie);
                RedirectToLoginUrl();
            }
        }

        private void RedirectToLoginUrl()
        {
            var httpContext = HttpContext.Current;
            var redirectUrl = LoginUrl + "?urlReferrer=" + httpContext.Request.Url.AbsolutePath;
            _logger.DebugFormat("正跳转到登录页：{0}", redirectUrl);
            httpContext.Response.Redirect(redirectUrl);
        }

        private void Login()
        {
            ////HACK:改成配置项 不要写死在程序中
            var httpContext = HttpContext.Current;
            var account = httpContext.Request["account"];
            var password = httpContext.Request["password"];
            var extension = httpContext.Request["extension"];
            var user = _userService.Authencation(account, password, extension);
            _logger.DebugFormat("正在验证登录信息：{0}", account);
            if (user != null)
            {
                httpContext.Items[HttpMoudlesConst.HttpUserKey] = user;
                var loginCookie = new HttpCookie(LoginCookieKey, _desCrypto.Encryptor(account)) { HttpOnly = true };
                var loginExtensionCookie = new HttpCookie(LoginExtensionCookieKey, extension) { HttpOnly = true };
                httpContext.Response.Cookies.Add(loginCookie);
                httpContext.Response.Cookies.Add(loginExtensionCookie);

                var urlReferrer = httpContext.Request.UrlReferrer;
                var urlrefInQuery = httpContext.Request["urlReferrer"];
                var refUrl = string.IsNullOrEmpty(urlrefInQuery) ? (urlReferrer != null ? urlReferrer.AbsoluteUri : string.Empty) : string.Empty;
                _logger.DebugFormat("urlrefInQuery：{0}", urlrefInQuery);
                _logger.DebugFormat("urlReferrer：{0}", urlReferrer);
                if (!string.IsNullOrEmpty(refUrl))
                {
                    if (refUrl.IndexOf(LoginUrl.Replace("~/", ""), System.StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        refUrl = ConvertUrl(HomeUrl);
                    }
                }
                _logger.DebugFormat("正跳转到：{0}", refUrl);
                httpContext.Response.Redirect(!string.IsNullOrEmpty(refUrl) ? refUrl : ConvertUrl(refUrl));
            }
            else
            {
                _logger.DebugFormat("用户名密码校验不通过：account={0};pwd={1},跳转到登录页并记录状态", account, password);
                httpContext.Response.Redirect(LoginUrl + string.Format("?status={0}", "0"));
            }
        }

        private static void RemoveLoginCookie(HttpContext httpContext, HttpCookie accountCookie, HttpCookie loginExtensionCookie)
        {
            accountCookie.Expires = DateTime.Now.AddYears(-1);
            httpContext.Response.Cookies.Add(accountCookie);
            if (loginExtensionCookie != null)
            {
                loginExtensionCookie.Expires = DateTime.Now.AddYears(-1);
                httpContext.Response.Cookies.Add(loginExtensionCookie);
            }
        }

        private void Logout()
        {
            _logger.DebugFormat("正在执行退出操作！");
            HttpContext httpContext = HttpContext.Current;
            var accountCookie = httpContext.Request.Cookies[LoginCookieKey];
            var loginExtensionCookie = httpContext.Request.Cookies[LoginExtensionCookieKey];
            if (accountCookie != null)
            {
                RemoveLoginCookie(httpContext, accountCookie, loginExtensionCookie);
                //跳转到新登录页
                //httpContext.Response.Redirect(LoginUrl + "?urlReferrer=" + httpContext.Request.Url.AbsoluteUri.Replace(string.Format("?{0}={1}", LogoutParam, LogoutValue), ""));
                var logoutUrl = httpContext.Request.Url.AbsoluteUri.Replace(string.Format("?{0}={1}", LogoutParam, LogoutValue), "");
                _logger.DebugFormat("退出成功，正跳转：{0}", logoutUrl);
                httpContext.Response.Redirect(logoutUrl);
            }
        }

        private string FormatUrlParam(string url, string parma)
        {
            var regex = new Regex(string.Format(@"[&*|\?*]{0}=[^&]*", parma), RegexOptions.IgnoreCase);
            return regex.Replace(url, string.Empty).Replace("?&", "?");
        }

        private static string ConvertUrl(string path)
        {
            var virtualPath = HostingEnvironment.ApplicationHost.GetVirtualPath();
            if (path.StartsWith("~/"))
            {
                if (!virtualPath.EndsWith("/"))
                    virtualPath = virtualPath + "/";
                return virtualPath + path.TrimStart(new char[] { '~', '/' });
            }
            return path;
        }

        #region 命令及命令值
        public string LogoutParam
        {
            get
            {
                var logoutParam = System.Configuration.ConfigurationManager.AppSettings["logoutCommand"];
                return string.IsNullOrEmpty(logoutParam) ? "cmd" : logoutParam;
            }
        }

        public string LogoutValue
        {
            get
            {
                var logoutValue = System.Configuration.ConfigurationManager.AppSettings["logoutValue"];
                return string.IsNullOrEmpty(logoutValue) ? "logout" : logoutValue;
            }
        }

        public string LoginParam
        {
            get
            {
                var logoutParam = System.Configuration.ConfigurationManager.AppSettings["LoginParam"];
                return string.IsNullOrEmpty(logoutParam) ? "cmd" : logoutParam;
            }
        }

        public string LoginValue
        {
            get
            {
                var logoutValue = System.Configuration.ConfigurationManager.AppSettings["LoginValue"];
                return string.IsNullOrEmpty(logoutValue) ? "Login" : logoutValue;
            }
        }

        public string LoginUrl
        {
            get
            {
                var str = System.Configuration.ConfigurationManager.AppSettings["LoginUrl"];
                if (string.IsNullOrEmpty(str))
                {
                    throw new ConfigurationErrorsException("必须配置LoginUrl的AppSettings键值作为登录地址");
                }
                return str;
            }
        }
        public string HomeUrl
        {
            get
            {
                var str = System.Configuration.ConfigurationManager.AppSettings["HomeUrl"];
                if (string.IsNullOrEmpty(str))
                {
                    throw new ConfigurationErrorsException("必须配置HomeUrl的AppSettings键值作为默认首页");
                }
                return str;
            }
        }
        public string LoginCookieKey
        {
            get
            {
                var loginCookieKey = System.Configuration.ConfigurationManager.AppSettings["LoginCookieKey"];
                return !string.IsNullOrEmpty(loginCookieKey) ? loginCookieKey : "accountKey";

            }
        }
        public string LoginExtensionCookieKey
        {
            get
            {
                var loginCookieKey = System.Configuration.ConfigurationManager.AppSettings["LoginExtensionCookieKey"];
                return !string.IsNullOrEmpty(loginCookieKey) ? loginCookieKey : "loginExtension";
            }
        }

        public string LoginPwdCookieKey
        {
            get
            {
                var loginPwdCookieKey = System.Configuration.ConfigurationManager.AppSettings["LoginPwdCookieKey"];
                return !string.IsNullOrEmpty(loginPwdCookieKey) ? loginPwdCookieKey : "accountValue";

            }
        }
        #endregion
    }
}
