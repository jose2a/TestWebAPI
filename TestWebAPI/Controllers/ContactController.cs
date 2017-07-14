using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using TestWebAPI.Models;

namespace TestWebAPI.Controllers
{
    [RoutePrefix("api/contact")]
    [EnableCors("*", "*", "*")]
    public class ContactController : ApiController
    {
        List<Contact> contacts = new List<Contact>
            {
                new Contact {Id = 1, FirstName = "Peter", LastName = "Parker" },
                new Contact {Id = 2, FirstName = "Bruce", LastName = "Wayne" },
                new Contact {Id = 3, FirstName = "Clark", LastName = "Kent" }
            };

        // GET: api/Contact
        public IHttpActionResult Get()
        {
            Request.CreateResponse().Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            return Ok(contacts);
        }

        // GET: api/Contact/5
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var contact = contacts.SingleOrDefault(c => c.Id == id);

            if (contact == null) { 
                return NotFound();
            }

            return Ok(contact);
        }

        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            var contact = contacts.FirstOrDefault(c => c.FirstName.ToLower().Contains(name.ToLower()));

            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        // POST: api/Contact
        public IHttpActionResult Post(Contact contact)
        {
            contact.Id = new Random().Next(10);
            contacts.Add(contact);

            return Created("/api/contact/" + contact.Id, contact);
        }

        // PUT: api/Contact/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Contact/5
        public void Delete(int id)
        {
        }
    }
}
