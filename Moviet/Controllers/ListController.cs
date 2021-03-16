using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moviet.Contracts;
using Moviet.Data;
using Moviet.Models;
using Moviet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Moviet.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ListController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IBanService _banService;
        private readonly IPostRepository _postrepo;

        public string certHeader = "";
        public string errorString = "";
        private X509Certificate2 certificate = null;
        public string certThumbprint = "";
        public string certSubject = "";
        public string certIssuer = "";
        public string certSignatureAlg = "";
        public string certIssueDate = "";
        public string certExpiryDate = "";
        public bool isValidCert = false;


        private bool IsValidClientCertificate()
        {
            // In this example we will only accept the certificate as a valid certificate if all the conditions below are met:
            // 1. The certificate is not expired and is active for the current time on server.
            // 2. The subject name of the certificate has the common name nildevecc
            // 3. The issuer name of the certificate has the common name nildevecc and organization name Microsoft Corp
            // 4. The thumbprint of the certificate is 30757A2E831977D8BD9C8496E4C99AB26CB9622B
            //
            // This example does NOT test that this certificate is chained to a Trusted Root Authority (or revoked) on the server 
            // and it allows for self signed certificates
            //

            if (certificate == null || !String.IsNullOrEmpty(errorString)) return false;

            // 1. Check time validity of certificate
            if (DateTime.Compare(DateTime.Now, certificate.NotBefore) < 0 || DateTime.Compare(DateTime.Now, certificate.NotAfter) > 0) return false;

            // 2. Check subject name of certificate
            bool foundSubject = false;
            string[] certSubjectData = certificate.Subject.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in certSubjectData)
            {
                if (String.Compare(s.Trim(), "CN=nildevecc") == 0)
                {
                    foundSubject = true;
                    break;
                }
            }
            if (!foundSubject) return false;

            // 3. Check issuer name of certificate
            bool foundIssuerCN = false, foundIssuerO = false;
            string[] certIssuerData = certificate.Issuer.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in certIssuerData)
            {
                if (String.Compare(s.Trim(), "CN=nildevecc") == 0)
                {
                    foundIssuerCN = true;
                    if (foundIssuerO) break;
                }

                if (String.Compare(s.Trim(), "O=Microsoft Corp") == 0)
                {
                    foundIssuerO = true;
                    if (foundIssuerCN) break;
                }
            }

            if (!foundIssuerCN || !foundIssuerO) return false;

            // 4. Check thumprint of certificate
            if (String.Compare(certificate.Thumbprint.Trim().ToUpper(), "30757A2E831977D8BD9C8496E4C99AB26CB9622B") != 0) return false;

            return true;
        }

        public ListController(UserManager<ApplicationUser> userManager,
                              IMapper mapper,
                              IBanService banService,
                              IPostRepository postrepo)
        {
            _userManager = userManager;
            _mapper = mapper;
            _banService = banService;
            _postrepo = postrepo;
        }

        public IActionResult AllUsers()
        {
            var raters = _userManager.GetUsersInRoleAsync(Roles.Rater).Result;
            var contentManager = _userManager.GetUsersInRoleAsync(Roles.ContentManager).Result;
            var allUsers = new List<ApplicationUser>();
            allUsers.AddRange(raters);
            allUsers.AddRange(contentManager);
            allUsers.Sort((p, q) => p.UserName.CompareTo(q.UserName));

            var model = _mapper.Map<List<IdentityUserVM>>(allUsers);

            foreach (var u in allUsers)
            {
                var role = _userManager.GetRolesAsync(u).Result.First();
                var m = model.Find(uvm => uvm.Id == u.Id);
                m.Role = role;
            }

            return View(model);
        }

        public IActionResult AllPosts()
        {
            var posts = _postrepo.FindAll();
            posts.Sort((p, q) => p.DateCreated.CompareTo(q.DateCreated));
            var model = _mapper.Map<List<PostVM>>(posts);

            return View(model);
        }

        public IActionResult BanUser(string id)
        {
            var banned = _banService.BanUser(id);
            if (!banned)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(AllUsers));

        }

        public IActionResult BanPost(int id)
        {
            var banned = _banService.BanPost(id);
            if (!banned)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(AllPosts));

        }
    }
}