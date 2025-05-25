// Services/AuthService.cs
public class AuthService
{
    public event Action? OnAuthChange;

    public void NotificarCambio() => OnAuthChange?.Invoke();
}
