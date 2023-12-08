using Microsoft.JSInterop;

namespace BlazorApp2.Services;

public class NetworkService(IJSRuntime jsRuntime)
{
    public async Task<bool> IsOnlineAsync() => await jsRuntime.InvokeAsync<bool>("checkOnlineStatus");
}