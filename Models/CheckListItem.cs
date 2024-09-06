using System.ComponentModel.DataAnnotations;
using CheckListAPI.Models;

public enum CheckListItemStatus
{
    Incomplete,
    InProgress,
    Completed
}

public class CheckListItem
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? CheckListItemName { get; set; }

    public int CheckListId { get; set; }

    public CheckList? CheckList { get; set; }

    // Tambahkan Status
    [Required]
    public CheckListItemStatus Status { get; set; } = CheckListItemStatus.Incomplete;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
