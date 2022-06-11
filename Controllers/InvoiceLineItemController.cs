using System.Data.Entity.Migrations;
using BurrisProject3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BurrisProject3.Controllers
{
    public class InvoiceLineItemController : Controller
    {

        public ActionResult InvoiceLineItems(string id, int sortBy = 0) {
            //creation of the entity and list of customers for future display
            BookEntities context = new BookEntities();
            List<InvoiceLineItem> iLineItem = context.InvoiceLineItems.ToList();

            switch (sortBy) {
                //default sorting will be by customer id
                case 0:
                default: {
                    iLineItem = context.InvoiceLineItems.OrderBy(c => c.InvoiceID).ToList();
                    break;
                }
                case 1: {
                    iLineItem = context.InvoiceLineItems.OrderBy(c => c.ProductCode).ToList();
                    break;
                }
                case 2: {
                    iLineItem = context.InvoiceLineItems.OrderBy(c => c.UnitPrice).ToList();
                    break;
                }
                case 3: {
                    iLineItem = context.InvoiceLineItems.OrderBy(c => c.Quantity).ToList();
                    break;
                }
                case 4: {
                    iLineItem = context.InvoiceLineItems.OrderBy(c => c.ItemTotal).ToList();
                    break;
                }
            } //end switch

            if (!string.IsNullOrWhiteSpace(id)) {
                id = id.Trim().ToLower();
                iLineItem = iLineItem.Where(i =>
                    i.ProductCode.ToLower().Contains(id) 
                ).ToList();
            }

            return View(iLineItem);
        }

        // GET: InvoiceLineItem
        [HttpGet]
        public ActionResult Upsert(int invoiceId, string productCode)
        {
            BookEntities context = new BookEntities();
            InvoiceLineItem lineItem = context.InvoiceLineItems.Where(i => i.InvoiceID == invoiceId && i.ProductCode == productCode).FirstOrDefault();

            if (lineItem == null) {
                lineItem = new InvoiceLineItem();
                lineItem.InvoiceID = invoiceId;
            }


            return View(lineItem);
        }

        [HttpPost]
        public ActionResult Upsert(InvoiceLineItem lineItem) {
            BookEntities context = new BookEntities();
            try {
                lineItem.ItemTotal = lineItem.Quantity * lineItem.UnitPrice;

                context.InvoiceLineItems.AddOrUpdate(lineItem);
                context.SaveChanges();
            }
            catch (Exception) {

                throw;
            }

            return Redirect("/Invoice/Upsert/" + lineItem.InvoiceID.ToString());
        }
    }
}