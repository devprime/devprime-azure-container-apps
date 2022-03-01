using System;

namespace Observability
{
    public class Metrics
    {
        //static void Prometheus(string[] args)
        //{
        //    var counter = Metrics.CreateCounter("peopleapi_path_counter", "Counts requests to the People API endpoints", new CounterConfiguration
        //    {
        //        LabelNames = new[] { "method", "endpoint" }
        //    });
        //    app.Use((context, next) =>
        //    {
        //        counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
        //        return next();
        //    });
        //}
    }
}
