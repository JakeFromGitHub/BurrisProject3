using BurrisProject3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Concurrent;
using System.Data.Entity.Migrations;

namespace BurrisProject3.Controllers {
    public class InvoiceController : Controller {
        // GET: Invoice
        public ActionResult Invoices() {
            BookEntities context = new BookEntities();

            List<Invoice> invoices = context.Invoices.ToList();

            return View(invoices);
        }

        [HttpGet]
        public ActionResult AddInvoice(int id = 0) {
            BookEntities context = new BookEntities();
            Invoice invoice = context.Invoices.Where(c => c.InvoiceID == id).FirstOrDefault();

            if (invoice == null) {
                invoice = new Invoice();
                invoice.Customer = new Customer();
            }

            InvoiceDTO dto = new InvoiceDTO() {
                Invoice = invoice,
                Customers = context.Customers.ToList(),
                Products = context.Products.ToList()
            };
            return View(dto);
        }

        /// <summary>
        /// Method used to Update/Insert an invoice based on the objects
        /// </summary>
        [HttpPost]
        public ActionResult AddInvoice(InvoiceDTO invoiceDTO, string customerId) {
            int iCustomerId = Convert.ToInt32(customerId.Split('-')[0].Trim());
            invoiceDTO.Invoice.CustomerID = iCustomerId;

            Invoice invoiceToUpdate = CalculateInvoiceTotals(invoiceDTO.Invoice);

            try {
                BookEntities context = new BookEntities();

                context.Invoices.AddOrUpdate(invoiceToUpdate);

                context.SaveChanges();
            }
            catch (Exception ex) {

                throw ex;
            }

            return RedirectToAction("Invoices");
        }

        //update invoice totals
        public Invoice CalculateInvoiceTotals(Invoice invoice) {
            BookEntities context = new BookEntities();
            List<InvoiceLineItem> lineItems = context.InvoiceLineItems.Where(i => i.InvoiceID == invoice.InvoiceID).ToList();

            invoice.InvoiceTotal = 0;
            foreach (var lineItem in lineItems) {
                invoice.ProductTotal = invoice.InvoiceTotal + lineItem.ItemTotal;
            }
            
            invoice.InvoiceTotal = invoice.ProductTotal + invoice.Shipping + invoice.SalesTax;

            return invoice;
        }

        [HttpPost]
        public ActionResult AddLineItem(InvoiceLineItem lineItem) {
            BookEntities context = new BookEntities();
            context.InvoiceLineItems.AddOrUpdate(lineItem);
            return Json("yes");
        }
    }
}