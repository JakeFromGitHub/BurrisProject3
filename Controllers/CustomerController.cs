using BurrisProject3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BurrisProject3.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Customers(string id, int sortBy = 0)
        {
            //creation of the entity and list of customers for future display
            BookEntities context = new BookEntities();
            List<Customer> customers = context.Customers.ToList();

            switch (sortBy) {
                //default sorting will be by customer id
                case 0:
                default: {
                    customers = context.Customers.OrderBy(c => c.CustomerID).ToList();
                    break;
                }
                case 1: {
                    customers = context.Customers.OrderBy(c => c.Name).ToList();
                    break;
                }
                case 2: {
                    customers = context.Customers.OrderBy(c => c.Address).ToList();
                    break;
                }
                case 3: {
                    customers = context.Customers.OrderBy(c => c.City).ToList();
                    break;
                }
                case 4: {
                    customers = context.Customers.OrderBy(c => c.State).ToList();
                    break;
                }
                case 5: {
                    customers = context.Customers.OrderBy(c => c.ZipCode).ToList();
                    break;
                }
            } //end switch

            if (!string.IsNullOrWhiteSpace(id)) {
                id = id.Trim().ToLower();
                customers = customers.Where(s =>
                    s.Name.ToLower().Contains(id) ||
                    s.Address.ToLower().Contains(id) ||
                    s.City.ToLower().Contains(id) ||
                    s.State.ToLower().Contains(id) 
                ).ToList();
            }

            return View(customers);
        }

        [HttpGet]
        public ActionResult AddCustomer(string id) {
            BookEntities context = new BookEntities();
            Customer customer = context.Customers.Where(c => c.Name == id).FirstOrDefault() ?? new Customer();

            //if (customer.IsDeleted) {
            //    return RedirectToAction("Customers");
            //}

            return View(customer);
        }

        [HttpPost]
        public ActionResult AddCustomer(Customer customer) {
            BookEntities context = new BookEntities();
            try {
                if (context.Customers.Where(c => c.Name == customer.Name).Count() > 0) {
                    var custToSave = context.Customers.Where(c => c.Name == customer.Name).ToList()[0];

                    custToSave.CustomerID = customer.CustomerID;
                    custToSave.Name = customer.Name;
                    custToSave.Address = customer.Address;
                    custToSave.City = customer.City;
                    custToSave.State = customer.State;
                    custToSave.ZipCode = customer.ZipCode;
                }
                else {
                    context.Customers.Add(customer);
                }


                context.SaveChanges();
            }
            catch (Exception ex) {
                throw ex;
            }

            return RedirectToAction("Customers");
        }

        //[HttpGet]
        //public ActionResult Delete(string id) {

        //}
    }
}