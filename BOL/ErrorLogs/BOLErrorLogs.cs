using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using DataAccess;
using System.Globalization;
using ParsetCrawler.DAL;

public class BOLErrorLogs : BaseBOLErrorLogs, IBaseBOL<ErrorLogs>
{
    public IList CheckBusinessRules()
    {
        var messages = new List<string>();
        return messages;
    }

    public void InsertLog(string ErrorDesc, string SectionName)
    {
        //ErrorsDataContext dc = new ErrorsDataContext();
        //ErrorLogs NewError = new ErrorLogs();
        //NewError.ErrorDesc = ErrorDesc;
        //NewError.ErrorTime = DateTime.Now;
        //NewError.SectionName = SectionName;

        //dc.ErrorLogs.InsertOnSubmit(NewError);
        //dc.SubmitChanges();
    }
}
