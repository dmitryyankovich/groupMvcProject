﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNet.Identity;
using WishList.ViewModels;

namespace WishList.Controllers
{
    public class GiftController : Controller
    {
        private readonly IGiftService giftService;

        public GiftController(IGiftService iGiftService)
        {
            giftService = iGiftService;
        }

        public ActionResult Catalog()
        {
            var gifts = giftService.GetAll().Select(Mapper.Map < DomainGift, GiftViewModel>).AsQueryable();
            return View(gifts);
        }

        public ActionResult TagPartial()
        {
            return PartialView();
        }

        public ActionResult CreateGift()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_CreateGiftPartial");
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateGift(CreateGiftViewModel createGiftViewModel)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Files["file"];

                var logoPath = "";

                if (file != null && file.ContentLength > 0)
                {
                    logoPath = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Server.MapPath(StringResources.GiftsLogoPath), logoPath);
                    file.SaveAs(path);
                }
                else
                {
                    logoPath = StringResources.NoGiftLogoPath;
                }

                createGiftViewModel.Logo = StringResources.GiftsLogoPath + logoPath;

                var newGift = Mapper.Map<DomainGift>(createGiftViewModel);
                giftService.Create(newGift);
                return RedirectToAction("CreateGiftSuccess");
            }

            ModelState.AddModelError("", "Invalid model");
            return View();
        }

        public ActionResult CreateGiftSuccess()
        {
            return View("CreateGiftSuccess");
        }

        public ActionResult UpdateGift(int id)
        {
            if (Request.IsAjaxRequest())
            {
                var gift = giftService.Get(id);

                return PartialView("_UdateGiftPartialView", Mapper.Map<GiftViewModel>(gift));
            }
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGift(GiftViewModel giftViewModel)
        {
            if (ModelState.IsValid)
            {
                var newGift = Mapper.Map<DomainGift>(giftViewModel);
                giftService.Update(newGift);
                return PartialView("_UpdateGiftSuccessPartial", giftViewModel.Name);
            }
            else
            {
                ModelState.AddModelError("", "Invalid model");
            }
            return PartialView("_UdateGiftPartialView");
        }

        public ActionResult DeleteGift(int id)
        {
            if (Request.IsAjaxRequest())
            {
                var giftName = giftService.Get(id).Name;
                giftService.Delete(id);
                return PartialView("_DeleteGiftSuccessPartial", giftName);
            }
            return View();
        }

        public ActionResult ViewGift(int id)
        {
            var gift = giftService.Get(id);
            var model = Mapper.Map<GiftViewModel>(gift);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public int ChangeLikesCount(string id)
        {
            return giftService.ChangeLikesCount(id, Int32.Parse(User.Identity.GetUserId()));
        }

        [HttpPost]
        public bool EnableLikes()
        {
            return User.Identity.IsAuthenticated;
        }
    }
}
