🎯 Incremental Rewrite Plan: Evolve Legacy .NET Framework WinForms Solution to Modern .NET + WPF
I reviewed the solution and codebase structure before drafting this plan.

Current baseline discovered:

Solution: Source/Evolution.sln with 2 projects:
Source/Evolution/Evolution.csproj (class library, .NET Framework 4.8)
Source/WindowsFormsApp1/Evolution.UI.csproj (WinForms app, .NET Framework 4.8)
UI references core via project reference (Evolution.UI.csproj → Evolution.csproj).
The core simulation logic is concentrated in Controller, Brain, Neuron, Decision, and extension-based mutation/clone helpers.
Existing persistence uses BinaryFormatter in both cloning and generation save/load:
Source/Evolution/Mutations/Extensions.cs (Clone<T>())
Source/Evolution/Controller.cs (SaveGeneration, LoadGeneration)
UI rendering currently uses immediate-mode GDI via CreateGraphics() and direct drawing in event handlers:
Source/WindowsFormsApp1/Form1.cs
Source/WindowsFormsApp1/Display.cs
No test project or CI assets were found in the solution.
You selected:

Primary path: incremental rewrite while preserving outputs
Runtime target: modern .NET (Windows-only)
UI direction: replace WinForms with WPF in first phase
Persistence compatibility: no need to preserve old .bin format
Test strictness: baseline smoke tests only
Given that direction, the safest plan is to separate concerns first, then migrate functionality in thin vertical slices. The highest-risk areas are (1) behavior drift in simulation outcomes, (2) replacing BinaryFormatter persistence, and (3) UI parity during WinForms → WPF transition.

Target architecture (incremental end state):

Evolution.Core (modern .NET class library): domain model + simulation engine
Evolution.Infrastructure (optional, or kept inside core initially): persistence and serialization adapters
Evolution.App.Wpf (modern .NET WPF desktop app): MVVM-based UI
Evolution.SmokeTests (minimal test project): deterministic and end-to-end smoke checks
Core behavior constraints to preserve:

Population sizing and selection strategy in Controller:
constants generationTotal = 500, generationMiddle = 250
top 1/5 parent selection and mutation mix in Evolve()
Step loop semantics:
DecideActions() → collision check → input refresh → neuron firing
Strategy count coupling:
decisions count = enum size (Strategies), neurons count = Strategies * 2
Scoring shape from Brain.FinalScore and ConvertFinalScore()
Technical modernization priorities:

Replace obsolete/unsafe serialization (BinaryFormatter) with explicit DTO-based JSON.
Remove reflection-driven runtime discovery as a hidden behavior dependency, replacing with explicit registries/factories.
Move UI logic out of code-behind drawing into view models + WPF rendering.
Establish deterministic execution mode (seed support preserved) for smoke checks.
Migration strategy (incremental slices):

Slice A: establish modern solution scaffold and compile green with placeholders.
Slice B: port core domain and engine with no intentional behavior changes.
Slice C: swap serialization to JSON DTOs and remove BinaryFormatter.
Slice D: introduce baseline smoke tests that lock critical behavior.
Slice E: implement WPF UI parity for current user flows.
Slice F: retire legacy WinForms and framework projects.
Risks and mitigations:

Risk: subtle simulation drift due to type/collection/order differences.
Mitigation: deterministic seeded smoke runs and score-distribution snapshots across several generations.
Risk: current code depends on reflection enumeration ordering in ControllerInit().
Mitigation: replace with explicit ordered registration lists for inputs/operators/comparators.
Risk: legacy bugs become “fixed by accident,” changing output.
Mitigation: preserve behavior first; backlog bug fixes as explicit post-parity changes.
Risk: UI migration may regress visualization behavior.
Mitigation: implement WPF feature parity against existing flows (new, evolve 1, evolve 25, simulate individuals, generation chart).
Definition of done for this rewrite:

Modern .NET Windows-only solution builds and runs from Visual Studio 2026.
WPF app reproduces key scenarios currently available in WinForms.
Persistence uses JSON (or equivalent modern serializer), no BinaryFormatter usage remains.
Smoke tests pass locally and in CI for deterministic seeded runs.
Legacy .NET Framework/WinForms projects archived or removed from active solution.
Last Updated: 2026-11-08 20:38:16

📝 Plan Steps
Create a modernization branch and capture baseline behavior — run the current WinForms app and document reference outputs (score distribution shape, winner/median/loser examples, generation progression after 1 and 25 evolves) for later parity checks.
Stand up a new SDK-style solution structure — add modern .NET projects (Evolution.Core, Evolution.App.Wpf, Evolution.SmokeTests) alongside legacy projects to enable side-by-side migration.
Port domain primitives into Evolution.Core — migrate Brain, Neuron, Decision, enums/interfaces, and input/operator/comparator types with behavior-preserving logic and explicit nullable/visibility cleanup only where non-behavioral.
Refactor engine orchestration out of Controller — split responsibilities into focused services (generation creation, step simulation, evolution, scoring, state updates) while preserving algorithm order and constants.
Replace reflection-based discovery with explicit registries — define ordered factories for inputs/operators/comparators to remove runtime-order instability and make behavior deterministic across runtimes.
Replace BinaryFormatter persistence with DTO JSON serialization — implement versioned save/load models for population and generation stats; remove binary clone hacks by introducing explicit deep-copy mechanics.
Add baseline smoke tests before UI completion — cover create/step/evolve/save-load flows with fixed seeds and assert non-null, count, monotonicity, and bounded score-distribution invariants.
Build WPF shell with MVVM — implement commands for “Create Population”, “Evolve 1”, “Evolve 25”, “Simulate Individuals”, and generation selection; bind to core services rather than direct drawing calls.
Reimplement visualizations in WPF controls — port the current brain text panels, step simulation view, and histogram chart from GDI drawing to retained-mode WPF rendering (Canvas/ItemsControl + data templates).
Wire persistence and session workflow in WPF — replace population.bin workflow with JSON save/load, defaulting to an app-data file path and adding explicit error handling and status feedback.
Perform parity validation and tuning — compare WPF/core outcomes against baseline snapshots for representative runs, then adjust only non-functional differences (formatting/layout/perf) until parity is acceptable.
Decommission legacy projects safely — remove WinForms project from startup, archive or delete .NET Framework projects, update solution configuration, and keep migration notes for future enhancements.
Add lightweight CI for smoke checks — configure build + smoke test execution on push/PR to protect the rewritten core and WPF app from regression.
Produce final upgrade documentation — include architecture overview, run instructions, save format details, known intentional deviations, and a short roadmap for post-migration improvements.