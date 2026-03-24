DOCKER = docker compose run --rm dotnet

# ── Build ──────────────────────────────────────────────
build:             ## Build le projet
	$(DOCKER) build TP2-SOLID-PRINCIPLES.sln

# ── Run ──────────────────────────────────────────────
run:               ## Exécute le programme
	$(DOCKER) run --project HotelReservation.csproj

# ── Nettoyage ─────────────────────────────────────────
clean:             ## Supprime les fichiers compilés (bin/ et obj/)
	find . -type d \( -name bin -o -name obj \) -exec rm -rf {} + 2>/dev/null || true

# ── Aide ──────────────────────────────────────────────
help:              ## Affiche cette aide
	@grep -E '^[a-zA-Z0-9_-]+:.*##' $(MAKEFILE_LIST) | \
		awk -F ':.*## ' '{printf "  \033[36m%-15s\033[0m %s\n", $$1, $$2}'

.PHONY: build run clean help
.DEFAULT_GOAL := help
