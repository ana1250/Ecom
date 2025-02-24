using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECom_Inventory.Model
{
	public class AuthLog
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		[MaxLength(100)]
		public string Username { get; set; }

		[Required]
		[MaxLength(50)]
		public string ActionType { get; set; }
		public DateTime ActionTimestamp { get; set; } = DateTime.UtcNow;

		[MaxLength(50)]
		public string? IPAddress { get; set; }

		[MaxLength(255)]
		public string? UserAgent { get; set; }
	}
}
