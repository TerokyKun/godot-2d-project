using Godot;

public partial class TeleportAbility : Node
{
    [Export] public float TeleportDistance = 220f;

    private Player _player;
    private PlayerUpgrades _upgrades;
    private float _cooldownTimer = 0f;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        _player = GetParent<Player>();
        _upgrades = _player?.GetNodeOrNull<PlayerUpgrades>("PlayerUpgrades");
    }

    public override void _Process(double delta)
    {
        float d = (float)delta;

        if (_cooldownTimer > 0f)
            _cooldownTimer -= d;

        if (_player == null || _upgrades == null)
            return;

        if (_upgrades.SelectedClass != PlayerClassType.Teleporter)
            return;

        if (Input.IsActionJustPressed("shift"))
            TryTeleport();
    }

    private void TryTeleport()
    {
        if (_player == null || _upgrades == null)
            return;

        if (_cooldownTimer > 0f)
            return;

        Vector2 dir = Vector2.Up.Rotated(_player.Rotation);
        _player.GlobalPosition += dir * TeleportDistance;
        _player.GrantInvulnerability(_upgrades.TeleportInvulnerability);

        _cooldownTimer = _upgrades.TeleportCooldown;
    }
}