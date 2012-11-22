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
        private ILog _logger;
        private const string HttpUserKey = "______httpUser______";
        private readonly IUserService _userService;
        private readonly DesCrypto _desCrypto;

        public AuthenticationHttpMoudle()
        {
            _userService = ServiceLocationHandler.Resolver<IUserService>();
            _desCrypto = new DesCrypto("hhyjuuhd", "mmnjikjh");
        }
        public void Dispose()
        {
            return;
        }

        public void Init(HttpApplication context)
        {
            _logger = new DefaultLoggerFactory().GetLogger();
            context.BeginRequest += Handler;
            context.EndRequest += SessionClose;
        }
        private static void SessionClose(object sender, EventArgs e)
        {
            var httpContext = HttpContext.Current;
            //if (httpContext != null)
            //    new DefaultSessionManagerFactory(new OracleSessionFactoryHelper().GetSessionFactory()).CreateManager().Dispose();
        }
        public void Handler(object sender, EventArgs args)
        {
            HttpContext httpContext = HttpContext.Current;
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
            if (accountCookie == null)
            {
                if (IsInIgronSettings(httpContext.Request))
                    return;
                if (!httpContext.Request.Url.AbsolutePath.Equals(LoginUrl.Replace("~", ""), StringComparison.CurrentCultureIgnoreCase))
                    RedirectToLoginUrl();
            }
            else if (httpContext.Request.Cookies["CorpId"] != null)
            {
                LoadUserProfie(accountCookie);
            }
        }

        private bool IsInIgronSettings(HttpRequest request)
        {
            var section = ConfigurationManager.GetSection("AuthencationSection") as AuthencationSection;
            if (section == null)
                throw new ConfigurationErrorsException("web.config中必须配置AuthencationSection节点信息");
            var isIngronePath = section.IgnorePaths.Cast<IgnorePathConfigurationElement>()
                .Any(igrone => request.Url.AbsolutePath.Trim() == "/" || request.Url.AbsolutePath.StartsWith(igrone.Path.Replace("~", ""), StringComparison.CurrentCultureIgnoreCase));
            if (!isIngronePath)
                return section.IgnorePostfixs.Cast<IgnorePostfixConfigurationElement>()
                   .Any(igronePostfix =>
                       request.FilePath
                       .EndsWith(igronePostfix.Postfix, StringComparison.CurrentCultureIgnoreCase));
            return true;
        }

        private void LoadUserProfie(HttpCookie accountCookie)
        {
            var httpContext = HttpContext.Current;
            var user = _userService.GetAuthencationUser(_desCrypto.Decryptor(accountCookie.Value));
            if (user != null)
                httpContext.Items[HttpUserKey] = user;
            else
            {
                RemoveLoginCookie(httpContext, accountCookie);
                RedirectToLoginUrl();
            }
        }

        private void RedirectToLoginUrl()
        {
            var httpContext = HttpContext.Current;
            httpContext.Response.Redirect(LoginUrl + "?urlReferrer=" + httpContext.Request.Url.AbsoluteUri);
        }

        private void Login()
        {
            ////HACK:改成配置项 不要写死在程序中
            var httpContext = HttpContext.Current;
            var account = httpContext.Request["account"];
            var password = httpContext.Request["password"];
            var user = _userService.Authencation(account, password);
            if (user != null)
            {
                httpContext.Items[HttpUserKey] = user;
                var loginCookie = new HttpCookie(LoginCookieKey, _desCrypto.Encryptor(account)) {HttpOnly = true};
                httpContext.Response.Cookies.Add(loginCookie);
              
                var urlReferrer = httpContext.Request.UrlReferrer;
                var urlrefInQuery = httpContext.Request["urlReferrer"];
                var refUrl = urlrefInQuery ?? (urlReferrer != null ? urlReferrer.AbsoluteUri : string.Empty);
                httpContext.Response.Redirect(!string.IsNullOrEmpty(refUrl) ? refUrl : ConvertUrl(refUrl));
            }
            else
            {
                httpContext.Response.Redirect(LoginUrl + string.Format("?status={0}", "0"));
            }
        }

        private void RemoveLoginCookie(HttpContext httpContext, HttpCookie accountCookie)
        {
            accountCookie.Expires = DateTime.Now.AddYears(-1);
            httpContext.Response.Cookies.Add(accountCookie);
            // httpContext.Request.Cookies.Remove(LoginCookieKey);
        }

        private void Logout()
        {
            HttpContext httpContext = HttpContext.Current;
            var accountCookie = httpContext.Request.Cookies[LoginCookieKey];
            if (accountCookie != null)
            {
                RemoveLoginCookie(httpContext, accountCookie);
                //跳转到新登录页
                //httpContext.Response.Redirect(LoginUrl + "?urlReferrer=" + httpContext.Request.Url.AbsoluteUri.Replace(string.Format("?{0}={1}", LogoutParam, LogoutValue), ""));
                httpContext.Response.Redirect(httpContext.Request.Url.AbsoluteUri.Replace(string.Format("?{0}={1}", LogoutParam, LogoutValue), ""));
            }
        }

        private string FormatUrlParam(string url, string parma)
        {
            var regex = new Regex(string.Format(@"[&*|\?*]{0}=[^&]*", parma), RegexOptions.IgnoreCase);
            return regex.Replace(url, string.Empty).Replace("?&", "?");
        }

        private string ConvertUrl(string path)
        {
            var virtualPath = HostingEnvironment.ApplicationHost.GetVirtualPath();
            if (path.StartsWith("~/"))
            {
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

        public string LoginCookieKey
        {
            get
            {
                var loginCookieKey = System.Configuration.ConfigurationManager.AppSettings["LoginCookieKey"];
                return !string.IsNullOrEmpty(loginCookieKey) ? loginCookieKey : "accountKey";

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
