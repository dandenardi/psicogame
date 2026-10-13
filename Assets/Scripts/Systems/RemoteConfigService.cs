namespace Freudinho.Systems
{
    /// <summary>
    /// Camada fina sobre Unity Remote Config para sobrescrever o BalanceConfig local em
    /// produção, sem precisar de nova build. Em dev/offline cai para o BalanceConfig local.
    /// </summary>
    public class RemoteConfigService
    {
    }
}
