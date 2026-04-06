`
docker build -t hoathse184053/puzkit3d-api:latest -f src/WebApi/PuzKit3D.WebApi/Dockerfile .
`


`
docker push hoathse184053/puzkit3d-api:latest
`


`
docker pull hoathse184053/puzkit3d-api:latest
`

`
docker container rm -f puzkit3d-api
`

`
docker run -d -p 80:80 --name puzkit3d-api hoathse184053/puzkit3d-api:latest
`

`
docker logs puzkit3d-api
`

`
docker stop puzkit3d-api
`
`
docker run -d \
  -p 80:80 \
  --name puzkit3d-api \
  -v /home/ec2-user/secrets/google-key.json:/app/secrets/google-key.json:ro \
  -e GOOGLE_APPLICATION_CREDENTIALS="/app/secrets/google-key.json" \
  -e ASPNETCORE_ENVIRONMENT="Production" \
  hoathse184053/puzkit3d-api:latest
`