using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Entities.ComplexTypes
{
    public enum FilterBy
    {
        [Display(Name = "Kategori")]
        Category = 0,
        [Display(Name = "Tarih")]
        Date = 1,
        [Display(Name = "Okunma sayısı")]
        ViewCount = 2,
        [Display(Name = "Yorum sayısı")]
        CommentCount = 3
    }
}
