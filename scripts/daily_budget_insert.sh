#!/bin/bash

curl "https://$SERVER_URL/Budget/InsertYesterdaysTransactions" \
  -H "Authorization: $CUSTYUP_API_KEY"