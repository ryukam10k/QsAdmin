using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QsAdmin.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        [DisplayName("登録者")]
        public string AccountId { get; set; }

        [DisplayName("タイトル")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("説明")]
        public string Description { get; set; }

        [DisplayName("担当者")]
        public string TantoshaId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("締切日時")]
        public DateTime? ShimekiriDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("完了日時")]
        public DateTime? KanryoDate { get; set; }

    }

    public class ToDoView
    {
        public List<MikanryoToDoView> MikanryoToDo { get; set; }

        public List<KanryoToDoView> KanryoToDo { get; set; }
    }

    public class MikanryoToDoView
    {
        public int Id { get; set; }

        [DisplayName("登録者")]
        public string AccountId { get; set; }

        [DisplayName("タイトル")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("説明")]
        public string Description { get; set; }

        [DisplayName("担当者")]
        public string TantoshaId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("締切日時")]
        public DateTime? ShimekiriDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("完了日時")]
        public DateTime? KanryoDate { get; set; }


        [DisplayName("担当者")]
        public string TantoshaName { get; set; }
    }

    public class KanryoToDoView
    {
        public int Id { get; set; }

        [DisplayName("登録者")]
        public string AccountId { get; set; }

        [DisplayName("タイトル")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("説明")]
        public string Description { get; set; }

        [DisplayName("担当者")]
        public string TantoshaId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("締切日時")]
        public DateTime? ShimekiriDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("完了日時")]
        public DateTime? KanryoDate { get; set; }


        [DisplayName("担当者")]
        public string TantoshaName { get; set; }
    }
}