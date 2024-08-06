namespace VyazmaTech.Platform.Rtc.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddRtc(this IHostApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment() is false)
            builder.Configuration.AddRtcProvider(builder.Environment, builder.Configuration);

        builder.Services.AddRtcServices(builder.Configuration, builder.Environment);

        return builder;
    }
}