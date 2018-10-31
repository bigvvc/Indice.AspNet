﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indice.Services
{
    /// <summary>
    /// settings used to bootstrap email service clients
    /// </summary>
    public class EmailServiceSettings
    {
        /// <summary>
        /// The configuration section name
        /// </summary>
        public static readonly string Name = "Email";
        /// <summary>
        /// The default sender address (ie no-reply@indice.gr)
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// The default sender name (INDICE OE)
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// The fqdn of the smtp server (ie mail.indice.gr)
        /// </summary>
        public string SmtpHost { get; set; }

        /// <summary>
        /// The port that the smtp server is listending
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// Toggles between http and https
        /// </summary>
        public bool UseSSL { get; set; }

        /// <summary>
        /// the <see cref="Username"/> to use on the credentials that will be sent over to consume the smtp service
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// the <see cref="Password"/> to use on the credentials that will be sent over to consume the smtp service. 
        /// This is optional in case we are inside a domain (smtp relay).
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// Abstraction for sending email through different providers and implementations. SMTP, SparkPost, Mailchimp etc.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="recipients">The recipients of the email message.</param>
        /// <param name="subject">The subject of the email message.</param>
        /// <param name="body">The body of the email message.</param>
        /// <returns></returns>
        Task SendAsync(string[] recipients, string subject, string body);

        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <typeparam name="TModel">The type of the <paramref name="data"/> that will be applied to the template.</typeparam>
        /// <param name="recipients">The recipients of the email message.</param>
        /// <param name="subject">The subject of the email message.</param>
        /// <param name="body">The body of the email message.</param>
        /// <param name="template">The template of the email message.</param>
        /// <param name="data">The data model that contains information to render in the email message.</param>
        /// <returns></returns>
        Task SendAsync<TModel>(string[] recipients, string subject, string body, string template, TModel data) where TModel : class;
    }
    
    /// <summary>
    /// Service extensions for <see cref="IEmailService"/>
    /// </summary>
    public static class EmailServiceExtensions
    {
        /// <summary>
        /// Send and email using a single recipient and default template with the name of "Email"
        /// </summary>
        /// <param name="emailService"></param>
        /// <param name="recipient"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static async Task SendAsync(this IEmailService emailService, string recipient, string subject, string body) =>
            await emailService.SendAsync<object>(new string[] { recipient }, subject, body, "Email", null);

        /// <summary>
        /// Send and email using a single recipient
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="emailService"></param>
        /// <param name="recipient"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="template"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task SendAsync<TModel>(this IEmailService emailService, string recipient, string subject, string body, string template, TModel data) where TModel : class =>
            await emailService.SendAsync(new string[] { recipient }, subject, body, template, data);
    }
}
