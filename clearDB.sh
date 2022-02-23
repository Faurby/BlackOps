#!/bin/bash

mongo MiniTwit --eval "db.Messages.remove({})"
mongo MiniTwit --eval "db.Users.remove({})"