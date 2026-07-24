namespace Okolitsa.Player // Keeps player-control architecture inside the OKOLITSA player namespace.
{
    public enum PlayerControlMode // Defines the reusable control states available to gameplay and Timeline sequences.
    {
        NoControl, // Blocks movement, camera look, interaction, and flashlight input.
        LookOnly, // Allows camera look but blocks movement, interaction, and flashlight input.
        FullControl // Allows normal player movement, camera look, interaction, and flashlight input.
    }
}