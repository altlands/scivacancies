﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Http;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacancySetReasonViewModel
    {
        [Required]
        public Guid Guid { get; set; }
        public VacancyDetailsViewModel Vacancy { get; set; }
        [Required]
        public string Reason { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
