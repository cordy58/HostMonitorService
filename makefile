# Variables
BACKEND_DIR=HostMonitor
HOSTS=google.com
PORT=80
INTERVAL=5

# Targets
.PHONY: all build run clean

all: build run

build:
	cd $(BACKEND_DIR) && dotnet build

run:
	@echo "Running backend on port $(PORT)..."
	cd $(BACKEND_DIR) && dotnet run -- --hosts=$(HOSTS) --port=$(PORT) --interval=$(INTERVAL)

clean:
	cd $(BACKEND_DIR) && dotnet clean
