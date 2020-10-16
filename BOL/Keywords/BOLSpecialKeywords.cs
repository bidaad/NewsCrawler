using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using DataAccess;
using System.Globalization;
using ParsetCrawler.DAL;

public class BOLSpecialKeywords : BaseBOLSpecialKeywords, IBaseBOL<SpecialKeywords>
{
    public IList CheckBusinessRules()
    {
        var messages = new List<string>();
        //Business rules here
        //if (this.ShowInFirstPage == true)
        //    if(this.HCLevelCode!=10)
        //    messages.Add("تنها اخبار با دسترسی عادی مجاز به نمایش در صفحه اول می باشند");
        return messages;
    }
}
