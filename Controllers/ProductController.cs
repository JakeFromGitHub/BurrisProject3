using BurrisProject3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BurrisProject3.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Products(string id, int sortBy = 0)
        {
            //creation of the entity and list of products for future display
            BookEntities context = new BookEntities();
            List<Product> products = context.Products.ToList();

            switch (sortBy) {
                //default sorting will be by product code
                case 0:
                default: {
                    products = context.Products.OrderBy(p => p.ProductCode).ToList();
                    break;
                }
                case 1: {
                    products = context.Products.OrderBy(c => c.Description).ToList();
                    break;
                }
                case 2: {
                    products = context.Products.OrderBy(c => c.UnitPrice).ToList();
                    break;
                }
                case 3: {
                    products = context.Products.OrderBy(c => c.OnHandQuantity).ToList();
                    break;
                }
            } //end switch

            if (!string.IsNullOrWhiteSpace(id)) {
                id = id.Trim().ToLower();
                products = products.Where(p =>
                    p.Description.ToLower().Contains(id) 
                ).ToList();
            }

            return View(products);
        }

        [HttpGet]
        public ActionResult AddProduct(string id) {
            BookEntities context = new BookEntities();
            Customer customer = context.Customers.Where(c => c.Name == id).FirstOrDefault();

            return View(customer);
        }

        [HttpPost]
        public ActionResult AddProduct(Product product) {
            BookEntities context = new BookEntities();
            try {
                if (context.Products.Where(p => p.ProductCode == product.ProductCode).Count() > 0) {
                    var prodToSave = context.Products.Where(p => p.ProductCode == product.ProductCode).ToList()[0];

                    prodToSave.ProductCode = product.ProductCode;
                    prodToSave.Description = product.Description;
                    prodToSave.UnitPrice = product.UnitPrice;
                    prodToSave.OnHandQuantity = product.OnHandQuantity;
                }
                else {
                    context.Products.Add(product);
                }


                context.SaveChanges();
            }
            catch (Exception ex) {
                throw ex;
            }

            return RedirectToAction("Products");
        }
    }
}