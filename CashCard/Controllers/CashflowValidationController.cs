﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CashCard.Models;
using Microsoft.AspNet.Identity;

namespace CashCard.Controllers
{
    public class CashflowValidationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public CashflowValidationController()
        {
            ViewBag.Menu = "MnCashflowValidation";
        }
        // GET: /WorkflowValidation/
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var branchId = db.Users.Find(userId).BranchId;

            var cashflows = db.CashFlows.Include(c => c.CutOff).Where(p => p.CutOff.BranchId == branchId && p.CutOff.State == StateCutOff.Start);
            return View(cashflows.ToList());
        }

        // GET: /WorkflowValidation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cashflow = db.CashFlows.OfType<CashOutRegular>().FirstOrDefault(p => p.Id == id);
            if (cashflow == null)
            {
                return HttpNotFound();
            }
            return View("CashoutRegular",cashflow);
        }

        [HttpPost]
        public JsonResult SetState(int idx, StateCashFlow statex)
        {
            try
            {
                var ch = db.CashFlows.Find(idx);
                switch (statex)
                {
                    case StateCashFlow.Revision:
                        ch.SetToRevision();
                        break;
                    case StateCashFlow.Reject:
                        ch.SetToReject();
                        break;
                    case StateCashFlow.Approve:
                        ch.SetToApprove();
                        break;
                    default:
                        throw new Exception("State not valid");
                }
                ch.SuperVisorId = User.Identity.GetUserId();
                db.SaveChanges();
                return Json(new { Success = 1, CashOutId = ch.Id, ex = "" });

            }
            catch (Exception ex)
            {

                return Json(new { Success = 0, ex = ex.Message });
             
            }
            
        }
       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
