﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlID.iDAccess
{
    public partial class Device
    {
        public string URL { get; private set; }
        public string Login { get; private set; }
        public int Port { get; private set; }
        public bool SSL { get; private set; }
        public string Session { get; private set; }
        public object Tag { get; set; } // Qualquer coisa (por exemplo uma classe que tem os dados de origem, ou identificadores proprios)
        public Exception LastError { get; private set; }

        private string Password;
        private DateTime dtLastCommand;
        private DateTime dtConnection;
        public int TimeOut;

        public Device(string cIP_DNS_URL = "http://192.168.0.129", string cLogin = "admin", string cPassword = "admin", bool useSSL = false, int nPort = 80, object oTag = null)
        {
            URL = cIP_DNS_URL;
            Login = cLogin;
            Password = cPassword;
            SSL = useSSL;
            if (SSL && nPort == 80)
                nPort = 443;
            Port = nPort;
            Tag = oTag;
            TimeOut = WebJson.TimeOut;
        }

        public void Connect(string cIP_DNS_URL = null, string cLogin = null, string cPassword = null, bool? useSSL = null, int? nPort = null)
        {
            URL = cIP_DNS_URL ?? URL;
            Login = cLogin ?? Login;
            Password = cPassword ?? Password;
            SSL = useSSL ?? SSL;
            Port = nPort ?? Port;

            // Sem dados não fa nada!
            if (URL == null || Login == null || Password == null)
                throw new cidException(ErroCodes.LoginRequestFields, "Invalid Request Start");

            // Limpa qualquer espaço desnecessário (evita erros de colagem)
            URL = URL.Trim().ToLower();

            // Foi passado o IP/DNS em vez da URL, então converte para a URL direto
            if (!URL.StartsWith("http") && !URL.Contains("://"))
            {
                if (SSL)
                    URL = "https://" + URL + (Port == 443 ? "" : (":" + Port));
                else
                    URL = "http://" + URL + (Port == 80 ? "" : (":" + Port));
            }

            // Deve ser sempre terminado por '/' pois os comandos serão concatenados diretamente
            if (!URL.EndsWith("/"))
                URL += "/";

            LoginRequest lreq = new LoginRequest();
            lreq.login = Login;
            lreq.password = Password;

            object result = WebJson.JsonCommand<LoginResult>(URL + "login.fcgi", lreq, null, TimeOut);
            if (result is LoginResult)
            {
                LoginResult dados = (LoginResult)result;
                if (dados.session == null)
                    throw new cidException(ErroCodes.LoginRequestFields, "Invalid User/Password");

                dtConnection = dtLastCommand = DateTime.Now;
                Session = dados.session;
            }
            else
                throw new cidException(ErroCodes.LoginInvalid, "Invalid Login");
        }

        public string TestConnect(string cURL = null, string cLogin = null, string cPassword = null, bool? useSSL = null, int? nPort = null)
        {
            try
            {
                Connect(cURL, cLogin, cPassword, useSSL, nPort);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void Disconnect()
        {
            string cLastSession = Session;
            Session = null;
            WebJson.JsonCommand<string>(URL + "logout.fcgi?session=" + cLastSession, null);
        }

        private void CheckSession()
        {
            if (dtConnection.Subtract(DateTime.Now).TotalHours > 23 ||
                dtLastCommand.Subtract(DateTime.Now).TotalHours > 3)
                Disconnect();

            if (Session == null)
                Connect();
        }
    }
}
