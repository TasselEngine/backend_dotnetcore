FROM microsoft/dotnet:2.0.0-runtime-deps

COPY sources.list /etc/apt

WORKDIR /app
COPY ubuntu.16.04-x64 /app
COPY run.sh /app

RUN apt-get update && apt-get install --allow-unauthenticated -y libfontconfig1
RUN ldd libSkiaSharp.so

# RUN dotnet restore
# RUN dotnet build

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000

ENTRYPOINT ["sh", "/app/run.sh"]
