FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR CreditCardPaymentSystem
COPY . .
WORKDIR CreditCardPaymentProcessor
RUN dotnet build "CreditCardPaymentProcessor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CreditCardPaymentProcessor.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CreditCardPaymentProcessor.dll"]