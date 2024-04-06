//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq;
//using System.Collections.Generic;
//using Twilio;
//using Twilio.Types;
//using Twilio.Rest.Api.V2010.Account;
//using panel1.Model;
//using Twilio.TwiML.Messaging;
//namespace panel1.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class whtspController : ControllerBase
//    {

//        [HttpPost]
//        public IActionResult Whsplibrary([FromBody] string message)
//        {
//            TwilioClient.Init(Settings.APIKEY, Settings.APISECRET);
//            PhoneNumber sender = new PhoneNumber("Whatsapp:+9050615686");
//            PhoneNumber target = new PhoneNumber("Whatsapp:"+Settings.phonenumber);
//            CreateMessageOptions options = new CreateMessageOptions(target);
//            options.From = sender;
//            options.Body = message;
//            MessageResource.Create(options);
//            //PhoneNumber target = new PhoneNumber("Whatsapp:+8168289398");

//            return Ok("message");
//        }
//    }

//}



using Microsoft.AspNetCore.Mvc;
using panel1.Model;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
namespace panel1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class whtspController : ControllerBase
    {
        [HttpPost]
        public IActionResult Whsplibrary([FromBody] MessageModel model)
        {
            TwilioClient.Init(Settings.APIKEY, Settings.APISECRET);
            PhoneNumber sender = new PhoneNumber("Whatsapp:+9050615686");
            PhoneNumber target = new PhoneNumber("Whatsapp:" + Settings.phonenumber);
            CreateMessageOptions options = new CreateMessageOptions(target);
            options.From = sender;
            options.Body = model.Message;
            MessageResource.Create(options);

            return Ok("Message sent successfully");
        }
    }

    public class MessageModel
    {
        public string Message { get; set; }
    }
}
