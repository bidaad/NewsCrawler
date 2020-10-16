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


public class BOLEntityRelations : BaseBOLEntityRelations, IBaseBOL<EntityRelations>
{

    public void SetRelatedEntities(int EntityCode, int HCEntityTypeCode, int HCRelatedEntityTypeCode, int MinMatualCount)
    {

        EntityRelationsDataContext datacontext;
        EntityRelationsDataContext datacontextInner;
        datacontext = new EntityRelationsDataContext();
        var ResultList = datacontext.spSetRelatedEntities(EntityCode, HCEntityTypeCode, HCRelatedEntityTypeCode, MinMatualCount);
        //var ResultList = datacontext.spGetRelatedEntities(EntityCode, HCEntityTypeCode, HCRelatedEntityTypeCode);
        //foreach (var item in ResultList)
        //{
        //    if (item.MatualCount >= MinMatualCount)
        //    {
        //        try
        //        {

        //            datacontextInner = new EntityRelationsDataContext();
        //            AtLeastOneOccurance = true;
        //            EntityRelations ER = new EntityRelations();
        //            datacontextInner.EntityRelations.InsertOnSubmit(ER);
        //            ER.EntityCode = EntityCode;
        //            ER.HCEntityTypeCode = HCEntityTypeCode;
        //            ER.RelatedEntityCode = item.EntityCode;
        //            ER.HCRelatedEntityTypeCode = HCRelatedEntityTypeCode;

        //            datacontextInner.SubmitChanges();
        //        }
        //        catch
        //        {
        //        }
        //    }
        //}

    }
    public IList CheckBusinessRules()
    {
        var messages = new List<string>();

        return messages;
    }
    public void SetAllRelations(int ReturnCode, int HCEntityCode, int MinMatualCount)
    {
        SetRelatedEntities(ReturnCode, HCEntityCode, 1, MinMatualCount); //Get Related News

/*
        SetRelatedEntities(ReturnCode, HCEntityCode, 2, MinMatualCount); //Get Related Persons
        SetRelatedEntities(ReturnCode, HCEntityCode, 3, MinMatualCount); //Get Related Parties
        SetRelatedEntities(ReturnCode, HCEntityCode, 4, MinMatualCount); //Get Related Countries
        SetRelatedEntities(ReturnCode, HCEntityCode, 5, MinMatualCount); //Get Related Articles
        SetRelatedEntities(ReturnCode, HCEntityCode, 6, MinMatualCount); //Get Related Centers
        SetRelatedEntities(ReturnCode, HCEntityCode, 7, MinMatualCount); //Get Related Documents
        SetRelatedEntities(ReturnCode, HCEntityCode, 8, MinMatualCount); //Get Related Lectures
        SetRelatedEntities(ReturnCode, HCEntityCode, 9, MinMatualCount); //Get Related Interviews
        SetRelatedEntities(ReturnCode, HCEntityCode, 10, MinMatualCount); //Get Related Thesises
        SetRelatedEntities(ReturnCode, HCEntityCode, 11, MinMatualCount); //Get Related Medias
        SetRelatedEntities(ReturnCode, HCEntityCode, 12, MinMatualCount); //Get Related Seminars

        SetRelatedEntities(ReturnCode, HCEntityCode, 14, MinMatualCount); //Get Related Albums
        SetRelatedEntities(ReturnCode, HCEntityCode, 15, MinMatualCount); //Get Related Minorities
        SetRelatedEntities(ReturnCode, HCEntityCode, 16, MinMatualCount); //Get Related Books
        SetRelatedEntities(ReturnCode, HCEntityCode, 17, MinMatualCount); //Get Related Journals
*/
    }


    public IQueryable<vRelatedNews> GetRelatedNews(int HCEntityTypeCode, int Code)
    {
        EntityRelationsDataContext datacontext = new EntityRelationsDataContext();
        return (from r in datacontext.vRelatedNews
                where r.EntityCode.Equals(Code) && r.HCEntityTypeCode.Equals(HCEntityTypeCode)
                orderby r.RelatedCode descending
                select r).Take(30);

    }
    public void DeleteReleatedEntity(int code, int relatedCode, string entityName)
    {
        var datacontext = new EntityRelationsDataContext();
        EntityRelations relation = null;
        switch (entityName)
        {

            case "vrelatednews":
                var rNews = datacontext.vRelatedNews.FirstOrDefault(c => c.Code.Equals(code) && c.RelatedCode.Equals(relatedCode));
                if (rNews != null)
                    relation = datacontext.EntityRelations.Single(r => r.RelatedEntityCode.Equals(relatedCode) && r.EntityCode.Equals(rNews.EntityCode));
                break;


            default:
                throw new Exception("No related entity found.");
        }

        if (relation != null)
        {
            datacontext.EntityRelations.DeleteOnSubmit(relation);
            datacontext.SubmitChanges();
        }
    }
    public IQueryable<vRelatedArticles> GetRelatedArticles(int HCEntityTypeCode, int Code)
    {
        EntityRelationsDataContext datacontext = new EntityRelationsDataContext();
        return (from r in datacontext.vRelatedArticles
                where (r.EntityCode.Equals(Code) && r.HCEntityTypeCode.Equals(HCEntityTypeCode))
                orderby r.RelatedCode descending
                select r
                    ).Take(30);
    }


}
