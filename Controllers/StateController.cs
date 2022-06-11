using BurrisProject3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BurrisProject3.Controllers
{
    public class StateController : Controller
    {
        // GET: State
        public ActionResult States(string id, int sortBy = 0)
        {
            //creation of the entity and list of states for future display
            BookEntities context = new BookEntities();
            List<State> states = context.States.ToList();

            switch (sortBy) {
                //default sorting will be by states code
                case 0:
                default: {
                    states = context.States.OrderBy(p => p.StateCode).ToList();
                    break;
                }
                case 1: {
                    states = context.States.OrderBy(c => c.StateName).ToList();
                    break;
                }
            } //end switch

            if (!string.IsNullOrWhiteSpace(id)) {
                id = id.Trim().ToLower();
                states = states.Where(s =>
                    s.StateName.ToLower().Contains(id)
                ).ToList();
            }

            return View(states);
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
