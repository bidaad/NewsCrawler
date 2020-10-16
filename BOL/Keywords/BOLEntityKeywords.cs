using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.SessionState;
using ParsetCrawler.DAL;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

public class BOLEntityKeywords : BaseBOLEntityKeywords, IBaseBOL<EntityKeywords>
{
    public void SaveKeywordList(int EntityCode, string KeywordCodeList, int EntityTypeCode)
    {
        string[] KeywordCodeListArray = KeywordCodeList.Split(',');
        if (KeywordCodeList != "")
        {
            BOLEntityKeywords EntityKeywordsBOL = new BOLEntityKeywords();
            for (int i = 0; i < KeywordCodeListArray.Length; i++)
            {
                EntityKeywordsBOL.KeywordCode = Convert.ToInt32(KeywordCodeListArray[i]);
                EntityKeywordsBOL.EntityCode = EntityCode;
                EntityKeywordsBOL.HCEntityTypeCode = EntityTypeCode;
                EntityKeywordsBOL.Deleted = false;
                int keywordEntity = GetKeywordEntity(EntityKeywordsBOL.KeywordCode, EntityKeywordsBOL.EntityCode, EntityKeywordsBOL.HCEntityTypeCode);
                //if (keywordEntity != 0) EntityKeywordsBOL.Code = keywordEntity;
                if (keywordEntity ==0)
                    EntityKeywordsBOL.SaveChanges(keywordEntity == 0);
            }
        }
    }

    private int GetKeywordEntity(int _KeywordCode, int _EntityCode, int _HCEntityTypeCode)
    {
        dataContext = new KeywordsDataContext();
        var a= from p in dataContext.EntityKeywords
                where p.KeywordCode == _KeywordCode && p.EntityCode == _EntityCode && p.HCEntityTypeCode == _HCEntityTypeCode
                select p.Code;

        return a.ToList().Count > 0 ? a.ToList()[0] : 0;

        
    }

    public IList CheckBusinessRules()
    {
        var messages = new List<string>();
        //Business rules here

        if (false)
            messages.Add("");

        return messages;
    }


    public void RemoveKeywordByCode(int KeywordCode)
    {
        KeywordsDataContext dc = new KeywordsDataContext();
        var dtlEntityKeywords = dc.EntityKeywords.Where(n => n.KeywordCode.Equals(KeywordCode));
        dc.EntityKeywords.DeleteAllOnSubmit(dtlEntityKeywords);
        dc.SubmitChanges();
    }
}
