using System.Collections.Generic;

public interface IPlayerProfile
{
    bool IsLocalProfile { get; set; }

    List<short> PlayerControllerIds { get; set; }
}
