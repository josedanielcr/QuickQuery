
# QuickQuery
This project is a .NET application featuring a trie-based search algorithm and Redis caching, optimized for efficient data retrieval using a master-slave database architecture. It excels in providing fast, optimized search suggestions, with a focus on popular queries. The application specifically targets a dataset of the cost of living index by country and cities, offering users quick access to global living cost data.


## Run locally

Clone the project

```bash
  git clone https://github.com/josedanielcr/QuickQuery.git
```

Create a docker network

```bash
  docker network create quickquerynetwork
```

**Main DB instance**

Navigate to the 'DB' directory and run the main PostgreSQL instance:

```bash
    docker run -d --rm --name QuickQuery-db1 `
    --net quickquerynetwork `
    -e POSTGRES_USER=postgresadmin `
    -e POSTGRES_PASSWORD=admin123 `
    -e POSTGRES_DB=quickquerydb `
    -e PGDATA="/data" `
    -v ${PWD}/QuickQuery-db1/pgdata:/data `
    -v ${PWD}/QuickQuery-db1/config:/config `
    -v ${PWD}/QuickQuery-db1/archive:/mnt/server/archive `
    -p 5000:5432 `
    postgres:16.1 -c 'config_file=/config/postgresql.conf'
```

**Setup replication**

Execute the following command to access the main database instance:
```bash
  docker exec -it QuickQuery-db1 bash
```

Create a user with replication capabilities and set a password for them:
```bash
  createuser -U postgresadmin -P -c 5 --replication replicationUser
```

Modify pg-hba.conf as follows:
```bash
  host     replication     replicationUser         0.0.0.0/0        md5
```

Update postgresql.conf with:
```bash
    wal_level = replica
    max_wal_senders = 3
    archive_mode = on
    archive_command = 'test ! -f /mnt/server/archive/%f && cp %p /mnt/server/archive/%f'
```

**Secondary DB instance**

Set up a data directory for the second instance with a backup from the main instance:
```bash
    docker run -it --rm `
    --net quickquerynetwork `
    -v ${PWD}/QuickQuery-db2/pgdata:/data `
    --entrypoint /bin/bash postgres:16.1
```

Run the following for base backup:
```bash
pg_basebackup -h QuickQuery-db1 -p 5432 -U replicationUser -D /data/ -Fp -Xs -R
```

Start the secondary instance
```bash
    docker run -d --rm --name QuickQuery-db2 `
    --net quickquerynetwork `
    -e POSTGRES_USER=postgresadmin `
    -e POSTGRES_PASSWORD=admin123 `
    -e POSTGRES_DB=quickquerydb  `
    -e PGDATA="/data" `
    -v ${PWD}/QuickQuery-db2/pgdata:/data `
    -v ${PWD}/QuickQuery-db2/config:/config `
    -v ${PWD}/QuickQuery-db2/archive:/mnt/server/archive `
    -p 5001:5432 `
    postgres:16.1 -c 'config_file=/config/postgresql.conf'
```
