using BurrisProject3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BurrisProject3.Controllers
{
    public class OrderOptionController : Controller
    {
        // GET: OrderOption
        public ActionResult OrderOptions(string id, int sortBy=0)
        {
            //creation of the entity and list of order options for future display
            BookEntities context = new BookEntities();
            List<OrderOption> options = context.OrderOptions.ToList();

            switch (sortBy) {
                //default sorting will be by first books shipping charge
                case 0:
                default: {
                    options = context.OrderOptions.OrderBy(o => o.FirstBookShipCharge).ToList();
                    break;
                }
                case 1: {
                    options = context.OrderOptions.OrderBy(o => o.SalesTaxRate).ToList();
                    break;
                }
                case 2: {
                    options = context.OrderOptions.OrderBy(o => o.AdditionalBookShipCharge).ToList();
                    break;
                }
            } //end switch

            return View(options);
        }

        [HttpGet]
        public ActionResult AddState(string id) {
            BookEntities context = new BookEntities();
            State state = context.States.Where(s => s.StateName == id).FirstOrDefault();

            return View(state);
        }

        [HttpPost]
        public ActionResult AddState(State state) {
            BookEntities context = new BookEntities();
            try {
                if (context.States.Where(s => s.StateCode == state.StateCode).Count() > 0) {
                    var stateToSave = context.States.Where(s => s.StateCode == state.StateCode).ToList()[0];

                    stateToSave.StateCode = state.StateCode;
                    stateToSave.StateName = state.StateName;
                }
                else {
                    context.States.Add(state);
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