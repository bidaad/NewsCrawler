using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Messages
/// </summary>
public static class Messages
{
    public static string ShowMessage(MessagesEnum msg)
    {
        string Message;
        switch (msg)
        {

            case MessagesEnum.SaveErrorGeneral:
                Message = "!خطا...اطلاعات ذخیره نشد";
                break;
            case MessagesEnum.LoadErrorGeneral:
                Message = "اطلاعات به درستی بارگذاری نشدند.";
                break;
            case MessagesEnum.InvalidBaseID:
                Message = "BaseID اشتباه است.";
                break;
            case MessagesEnum.InvalidID:
                Message = "شناسه صحیح نیست.";
                break;
            case MessagesEnum.SavedSuccessfuly:
                Message = "اطلاعات با موفقیت ذخیره شد.";
                break;
            case MessagesEnum.PrimaryInfomationSavedSuccessfuly:
                Message = "اطلاعات اولیه با موفقیت ذخیره شد.حال می توانید اطلاعات تکمیلی را ذخیره نمایید";
                break;
            case MessagesEnum.InvalidLogin:
                Message = "نام کاربری یا کلمه عبور اشتباه است";
                break;
            case MessagesEnum.DeletedSuccessfuly:
                Message = "رکورد مورد نظر حذف شد.";
                break;
            case MessagesEnum.PassandConfDoNotMatch:
                Message = "کلمه عبور جدید و تایید کلمه عبور یکی نیستند";
                break;
            case MessagesEnum.InvalidPassword:
                Message = "کلمه عبور اشتباه است";
                break;
            case MessagesEnum.PleaseSelectYourRole:
                Message = "لطفا نقش خود را انتخاب کنید";
                break;
            case MessagesEnum.TraspasserNotSet:
                Message = "شخص متخلف مشخص نشده است";
                break;
            case MessagesEnum.InformationSuccessfullySaved:
                Message = "اطلاعات با موفقیت ذخیره شد";
                break;
            case MessagesEnum.PasswordSuccessfullyChanged:
                Message = "کلمه عبور با موفقیت تغییر کرد";
                break;
            case MessagesEnum.InvalidUsernameORPassword:
                Message = "نام کاربری یا کلمه عبور اشتباه است";
                break;
            case MessagesEnum.UnableToAddDuplicateRecord:
                Message = "خطا در ذخیره اطلاعات - رکورد تکراری";
                break;
            case MessagesEnum.ErrorSavingData:
                Message = "بروز خطا در ذخیره اطلاعات";
                break;
            case MessagesEnum.ContentRateReCalculated:
                Message = "امتیاز محتواهای این نشریه مجددا محاسبه شد";
                break;
            case MessagesEnum.ErrorWhileDelete:
                Message = "رکورد مرتبط دارای اطلاعات مرتبط میباشد و قابل حذف نیست";
                break;
            case MessagesEnum.ErrorInsertDuplicate:
                Message = "ایجاد رکورد تکراری با خطا مواجه شد";
                break;
            case MessagesEnum.ErrorDuplicateJournalContent:
                Message = "محتوایی با این عنوان قبلا وارد شده است";
                break;
            case MessagesEnum.ErrorPleaseFillJournalField:
                Message = "لطفا فیلد نشریه را پر کنید";
                break;
            case MessagesEnum.ErrorPleaseFillCommentField:
                Message = "لطفا فیلد نظر را پر کنید";
                break;
            case MessagesEnum.InfoYourCommentSuccessfullySaved:
                Message = "نظر شما با موفقیت ثبت شد";
                break;
            case MessagesEnum.ErrorNotValidFileName:
                Message = "پسوند فایل معتبر نیست";
                break;
            case MessagesEnum.ErrorNoAccess:
                Message = "شما به اطلاعات این بخش دسترسی ندارید";
                break;
                
                
            default:
                Message = "";
                break;
        }
        return Message;
    }
}
public enum MessagesEnum
{
    SaveErrorGeneral,
    LoadErrorGeneral,
    InvalidBaseID,
    InvalidID,
    SavedSuccessfuly,
    PrimaryInfomationSavedSuccessfuly,
    InvalidLogin,
    DeletedSuccessfuly,
    PassandConfDoNotMatch,
    PasswordSuccessfullyChanged,
    InvalidPassword,
    PleaseSelectYourRole,
    TraspasserNotSet,
    InformationSuccessfullySaved,
    NewPassAndConfPassDoNotMatch,
    YourCurrentPasswordIsInvalid,
    UsernameTaken,
    FileExtensionIsInvalid,
    FileSizeExeed,
    InvalidUsernameORPassword,
    UnableToAddDuplicateRecord,
    ErrorSavingData,
    ContentRateReCalculated,
    ErrorWhileDelete,
    ErrorInsertDuplicate,
    ErrorDuplicateJournalContent,
    ErrorPleaseFillJournalField,
    ErrorPleaseFillCommentField,
    InfoYourCommentSuccessfullySaved,
    ErrorNotValidFileName,
    ErrorNoAccess

}

