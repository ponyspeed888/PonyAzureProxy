using Microsoft.AspNetCore.Http.Features;

namespace PonyAzureProxy
{
    internal class MyDiagnosticListener : IObserver<KeyValuePair<string, object>>, IDisposable
    {
        private readonly IDisposable _subscription;
        private readonly Action<KeyValuePair<string, object>> _callback;
        private static string[] DiagnosticKeys;


        public MyDiagnosticListener(DiagnosticListener diagnosticListener, string[] diagnosticKeys, Action<KeyValuePair<string, object>> callback)
        {
            DiagnosticKeys = diagnosticKeys;
            _subscription = diagnosticListener.Subscribe(this!, IsEnabled);
            _callback = callback;
        }
        private static readonly Predicate<string> IsEnabled = (provider) =>
        {
            if (DiagnosticKeys.Contains(provider))
                return true;

            return false;
        };



        public static void DumpPair(KeyValuePair<string, object> pair)
        {
            Debug.WriteLine($"DumpPair : {pair.Key} = {pair.Value.ToString()}");
        }

        public static void ProcessBadRequest(KeyValuePair<string, object> pair, Action<IBadRequestExceptionFeature> _callback)
        {
            if (pair.Value is IFeatureCollection featureCollection)
            {
                var badRequestFeature = featureCollection.Get<IBadRequestExceptionFeature>();

                if (badRequestFeature is not null)
                {
                    _callback(badRequestFeature);
                }
            }


        }

        public void OnNext(KeyValuePair<string, object> pair)
        {
            //DumpPair(pair);

            _callback(pair);



        }
        public void OnError(Exception error)
        {
            Debug.WriteLine("error : " + error.Message);
        }
        public void OnCompleted()
        {
            Debug.WriteLine("compelte");

        }
        public virtual void Dispose() => _subscription.Dispose();
    }
}