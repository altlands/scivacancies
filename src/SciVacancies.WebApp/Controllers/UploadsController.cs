using System;
using System.IO;
using System.Linq;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using System.Net;
using MediatR;
using Microsoft.AspNet.Authorization;
using SciVacancies.WebApp.Queries;

namespace SciVacancies.WebApp.Controllers
{
    [Route("uploads")]
    public class UploadsController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMediator _mediator;

        public UploadsController(IHostingEnvironment hostingEnvironment, IMediator mediator)
        {
            _hostingEnvironment = hostingEnvironment;
            _mediator = mediator;
        }

        /// <summary>
        /// получить файл - фото Заявителя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="extension"></param>
        /// <param name="researcherGuid"></param>
        /// <param name="organizationGuid"></param>
        /// <returns></returns>
        [Authorize]
        [Route("researcherphoto/{id}/{extension}")]
        public IActionResult ResearcherPhoto(string id, string extension, Guid researcherGuid, Guid organizationGuid)
        {
            var fullDirectoryPath = $"{_hostingEnvironment.WebRootPath}/uploads/researcherphoto/";
            return GetFile(fullDirectoryPath,id, extension);
        }

        /// <summary>
        /// получить файл, прикрепленный к Заявке
        /// </summary>
        /// <param name="pathGuid"></param>
        /// <param name="filename"></param>
        /// <param name="extension"></param>
        /// <param name="researcherGuid"></param>
        /// <param name="organizationGuid"></param>
        /// <returns></returns>
        [Authorize]
        [Route("applications/attachments/{pathGuid}/{filename}/{extension}")]
        [BindOrganizationIdFromClaims]
        [BindResearcherIdFromClaims]
        public IActionResult Applications(string pathGuid, string filename, string extension, Guid researcherGuid, Guid organizationGuid)
        {
            if (string.IsNullOrEmpty(pathGuid))
                return null;

            if (!User.Identity.IsAuthenticated)
                return HttpUnauthorized();

            filename = WebUtility.UrlDecode(filename);

            var vacancyApplicationAttachment = _mediator.Send(new SingleVacancyApplicationAttachmentByPathGuidQuery { UrlPath= pathGuid, FileName = filename });
            if (vacancyApplicationAttachment == null)
                return HttpNotFound($"{nameof(vacancyApplicationAttachment)} not found");

            var application = _mediator.Send(new SingleVacancyApplicationQuery { VacancyApplicationGuid = vacancyApplicationAttachment.vacancyapplication_guid });
            if (application == null)
                return HttpNotFound($"{nameof(application)} not found");

            var isAuthorizationSuccessful = false;

            //если Исследователь
            if (User.IsInRole(ConstTerms.RequireRoleResearcher))
            {
                //проверка на пустое значение
                if (researcherGuid == Guid.Empty)
                    return HttpUnauthorized();

                //Исследователь может получать файлы, прикрепленный только к своим Заявкам
                if (application.researcher_guid != researcherGuid)
                    return HttpBadRequest("Attempt to get file from another user");

                isAuthorizationSuccessful = true;
            }

            //если Организация
            if (User.IsInRole(ConstTerms.RequireRoleOrganizationAdmin))
            {
                //проверка на пустое значение
                if (organizationGuid == Guid.Empty)
                    return HttpUnauthorized();

                //Организация может получать файлы, прикрепленные к Заявкам на свои вакансии
                var vacancy = _mediator.Send(new SingleVacancyQuery { VacancyGuid = application.vacancy_guid });
                if (vacancy == null)
                    return HttpNotFound("Not found linked Vacancy");
                if (vacancy.organization_guid != organizationGuid)
                    return HttpBadRequest("Attempt to get file from Application applied to Vacancy from another organization");

                isAuthorizationSuccessful = true;
            }

            if (!isAuthorizationSuccessful)
                return HttpBadRequest("You have not access to the current file");

            var fullDirectoryPath = $"{_hostingEnvironment.WebRootPath}/uploads/applications/attachments/{pathGuid}";
            return GetFile(fullDirectoryPath, filename, extension);
        }

        /// <summary>
        /// получиьт файл, прикрепленный к вакансии
        /// </summary>
        /// <param name="pathGuid"></param>
        /// <param name="extension"></param>
        /// <param name="filename"></param>
        /// <param name="researcherGuid"></param>
        /// <param name="organizationGuid"></param>
        /// <returns></returns>
        [Route("vacancies/attachments/{pathGuid}/{filename}/{extension}")]
        [BindOrganizationIdFromClaims]
        [BindResearcherIdFromClaims]
        public IActionResult Vacancies(string pathGuid, string filename, string extension, Guid researcherGuid, Guid organizationGuid)
        {
            if (string.IsNullOrEmpty(pathGuid))
                return null;

            filename = WebUtility.UrlDecode(filename);

            var vacancyAttachment=_mediator.Send(new SingleVacancyAttachmentByPathGuidQuery { UrlPath = pathGuid, FileName = filename});
            if (vacancyAttachment == null)
                return HttpNotFound($"{nameof(vacancyAttachment)} not found");

            var vacancy = _mediator.Send(new SingleVacancyQuery{ VacancyGuid = vacancyAttachment.vacancy_guid });
            if (vacancy == null)
                return HttpNotFound($"{nameof(vacancy)} not found");

            var fullDirectoryPath = $"{_hostingEnvironment.WebRootPath}/uploads/vacancies/attachments/{pathGuid}";
            return GetFile(fullDirectoryPath, filename, extension);
        }

        /// <summary>
        /// получить и отправить файл
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="extension"></param>
        /// <param name="fullDirectoryPath"></param>
        /// <returns></returns>
        private IActionResult GetFile(string fullDirectoryPath, string filename, string extension)
        {
            var fullPathToFile = $@"{fullDirectoryPath}/{filename}.{extension}";
            if (!System.IO.File.Exists(fullPathToFile))
                return HttpNotFound();

            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPathToFile);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, $"{filename}.{extension}");
        }
    }
}
