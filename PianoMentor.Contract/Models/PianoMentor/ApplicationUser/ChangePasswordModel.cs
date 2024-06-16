using System.Security.Claims;

namespace PianoMentor.Contract.Models.PianoMentor.ApplicationUser;

public class ChangePasswordModel
{
    public long UserId { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string RepeatNewPassword { get; set; }
}