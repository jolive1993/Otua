using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Outa.Models;

namespace Outa.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Outa.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Request>("RequestsA");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class RequestsAController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/RequestsA
        [EnableQuery]
        public Request GetRequestsA()
        {
            return db.Requests.First();
        }

        // GET: odata/RequestsA(5)
        [EnableQuery]
        public SingleResult<Request> GetRequest([FromODataUri] int key)
        {
            return SingleResult.Create(db.Requests.Where(request => request.Id == key));
        }

        // PUT: odata/RequestsA(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Request> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Request request = db.Requests.Find(key);
            if (request == null)
            {
                return NotFound();
            }

            patch.Put(request);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(request);
        }

        // POST: odata/RequestsA
        public IHttpActionResult Post(Request request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Requests.Add(request);
            db.SaveChanges();

            return Created(request);
        }

        // PATCH: odata/RequestsA(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Request> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Request request = db.Requests.Find(key);
            if (request == null)
            {
                return NotFound();
            }

            patch.Patch(request);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(request);
        }

        // DELETE: odata/RequestsA(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Request request = db.Requests.Find(key);
            if (request == null)
            {
                return NotFound();
            }

            db.Requests.Remove(request);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RequestExists(int key)
        {
            return db.Requests.Count(e => e.Id == key) > 0;
        }
    }
}
