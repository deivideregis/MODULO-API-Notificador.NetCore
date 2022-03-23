using APINotificador.NetCore.Dominio.EmailRoot;
using APINotificador.NetCore.Infra.Data.Core.Context;
using APINotificador.NetCore.Infra.Data.Core.Repository.Base;
using APINotificador.NetCore.Infra.Data.Core.Repository.Interfaces.Emails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace APINotificador.NetCore.Infra.Data.Core.Repository.Emails
{
    public class EmailEnviarRepository : Repository<Email>, IEmailEnviarRepository
    {
        public EmailEnviarRepository(ContextBase context) : base(context)
        {
        }

        public async Task<bool> EnviarEmail(Email model)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(model.EmailCorporativa, model.NomeCorporativa);

            for (int i = 0; i < model.ListaDestinatario.Count; i++)
            {
                message.To.Add(new MailAddress(model.ListaDestinatario[i]));
            }

            message.Subject = model.Assunto;

            if (model.Anexo != null)
            {
                foreach (var file in model.Anexo)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            Attachment att = new Attachment(new MemoryStream(fileBytes), file.FileName);
                            message.Attachments.Add(att);
                        }
                    }
                }
            }

            message.IsBodyHtml = true;
            message.Body = model.Corpo;
            smtp.Port = model.PortaRemetente;
            smtp.Host = model.ServidorRemetente;
            smtp.EnableSsl = model.SslRemetente;
            smtp.UseDefaultCredentials = model.EUsarCredencial;
            smtp.Credentials = new NetworkCredential(model.EmailCorporativa, model.SenhaCorporativa);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Timeout = 100000;
            
            try
            {
                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                model.Corpo = ex.Message;

                return false;
            }

            return true;
        }
    }
}
