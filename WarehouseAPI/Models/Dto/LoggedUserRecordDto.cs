namespace WarehouseAPI.Models.Dto
{
	public class LoggedUserRecordDto
	{
		public bool IsLogged { get; set; }
		public User UserItem { get; set; }
		public string Token { get; set; }
		public string Message { get; set; }
	}
}
