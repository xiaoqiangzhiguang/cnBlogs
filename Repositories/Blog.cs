
using  System;
namespace Repositories
{
	public class BlogEntity
	{
		public BlogEntity()
		{
				Content = string.Empty;
			Author = string.Empty;
			Title = string.Empty;
			CreateTime = DateTime.Parse("1900-01-01");
			IsDelete = 0;
		}
		/// <summary>
		/// 主键
		/// </summary>
		public long Id { get;set;}
		/// <summary>
		/// 内容
		/// </summary>
		public string Content { get;set;}
		/// <summary>
		/// 作者
		/// </summary>
		public string Author { get;set;}
		/// <summary>
		/// 标题
		/// </summary>
		public string Title { get;set;}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateTime { get;set;}
		/// <summary>
		/// 修改时间
		/// </summary>
		public DateTime UpdateTime { get;set;}
		/// <summary>
		/// 是否删除（0--未删除，1--删除）
		/// </summary>
		public int IsDelete { get;set;}
		
	}
}

