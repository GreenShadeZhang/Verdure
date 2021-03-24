using System;
using System.Collections.Generic;
using System.Text;

namespace Verdure.ApplicationCore
{
   public class ArticleListDto
    {
        public int PageTotal { get; set; }
        public List<ArticleDto> Arts { get; set; }
    }
}
