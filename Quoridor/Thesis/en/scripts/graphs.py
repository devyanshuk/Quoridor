#!/bin/python3

import concurrent.futures, argparse, subprocess
import matplotlib.pyplot as plt

def run_subprocess(dim, depth, s1, args):
    cmd = [
        "dotnet",
        "run",
        "play",
        f"-s1={s1}",
        f"-depth={depth}",
        f"-s2={args.s2}",
        "-b",
        "-verbose=false",
        f"-numsim={args.simulations}",
        f"-dimension={dim}"
    ]

    result = subprocess.run(cmd, cwd="../../../Quoridor.ConsoleApp/", capture_output=True)

    #SAMPLE OUTPUT
    
    #------------------------------------------average s1 run time--------------------------------v
    """
    ===Results===
    Player A : MinimaxAlgorithmABPruning won 1/1 games. Win rate : 100%. Average move time(ms) : 1024.8
    Player B : Semi-Random won 0/1 games. Win rate : 0%. Average move time(ms) : 3.68
    Toal moves made across 1 games : 39
    Average total moves per game : 39
    Average branching factor : 69.10256
    """
    #-avg branching factor--------^

    if args.verbose:
        print()
        print(f"{dim}x{dim}, depth : {depth}, {s1} vs {args.s2}:")
        for line in result.stdout.splitlines():
            print(line.decode("utf-8"))

    info_extract = lambda line : float(result.stdout.splitlines()[line].split()[-1].strip().decode("utf-8"))

    branching_factor = info_extract(-1)
    time_s1_took = info_extract(1)
    
    return s1, dim, depth, branching_factor, time_s1_took

def save_branching_factor_fig(dim_map):
    xs = sorted(dim_map)
    ys = [dim_map[i] for i in xs]
    min_bfs = [1]*len(xs)
    max_bfs = [(5 + 2 * (i - 1)**2) for i in xs]
    plt.xticks(xs)
    plt.xlim(min(xs), max(xs))
    plt.xlabel("Board dimension")
    plt.ylabel("Branching factor")
    plt.plot(xs, ys, marker='o', label="average")
    plt.plot(xs, min_bfs, label="minimum")
    plt.plot(xs, max_bfs, marker='x', label="maximum")
    plt.legend()
    plt.grid()
    plt.savefig("../../img/branching_factor.png")
    plt.show()

def save_time_fig(time_map):
    fig, axes = plt.subplots(nrows=2, ncols=2, figsize=(12, 8))
    axes = axes.flatten()

    dimensions = sorted(set(dim for agent in time_map for dim in time_map[agent]))
    for i, dim in enumerate(dimensions):
        all_depths = set()
        for strategy in time_map:
            if dim in time_map[strategy]:
                depths, times = zip(*sorted(time_map[strategy][dim]))
                axes[i].plot(depths, times, marker='o', label=strategy)
                all_depths.update(depths)

        axes[i].set_title(f'Dimension {dim}')
        axes[i].set_xlabel('Depth')
        axes[i].set_ylabel('Time (ms)')
        axes[i].set_xticks(sorted(all_depths))
        axes[i].legend()
        axes[i].grid(True)

    plt.tight_layout()
    plt.savefig("../../img/performance.png")
    plt.show()

def main(args):
    dims_to_examine = list(map(int, args.dims_to_examine.split(",")))
    depths_to_examine = list(map(int, args.depths.split(",")))
    agent1s = args.s1.split(",")

    if args.verbose:
        print(f"Examining params\nDimensions: {dims_to_examine}\nDepths: {depths_to_examine}\nAgents: {agent1s}")

    with concurrent.futures.ProcessPoolExecutor() as executor:
        futures = []
        for dim in dims_to_examine:
            for depth in depths_to_examine:
                for s1 in agent1s:
                    futures.append(executor.submit(run_subprocess, dim, depth, s1, args))

        for future in concurrent.futures.as_completed(futures):
            agent, dim, dep, bf, time = future.result()
            if agent not in time_map:
                time_map[agent] = {}
            if not dim in time_map[agent]:
                time_map[agent][dim] = []
            time_map[agent][dim].append((dep, time))

            if dim not in bf_map:
                bf_map[dim] = []
            bf_map[dim].append(bf)

            if args.verbose:
                print(f"{dim}x{dim} board. Agent : {s1}, Time: {time}, Avg Branching factor : {bf}")

            print(time_map)

    if args.verbose:
        print()
        print(f"Time map : {time_map}")
        print(f"Branching factor map : {bf_map}")

    dim_map = {}
    for i in bf_map:
        dim_map[i] = sum(bf_map[i]) / len(bf_map[i])

    print(dim_map)
    save_time_fig(time_map)
    save_branching_factor_fig(dim_map)

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument('--s1', type=str, default="minimaxab,minimax,parallelminimaxab" ,help='Strategies for player 1')
    parser.add_argument('--depths', type=str, default="1,2,3", help='minimax depth')
    parser.add_argument('--s2', type=str, default="semirandom", help='Strategy for player 2')
    parser.add_argument('--simulations', type=int, default=1, help='Number of simulations')
    parser.add_argument("--verbose", action="store_true", default=False, help='verbose flag. diaplay braching factor, time results')
    parser.add_argument("--dims_to_examine", type=str, default="3,5,7,9", help='dimensions to examine branching factor and times for')
    args = parser.parse_args()
    main(args)


"""
Time map : {'minimaxab': {3: [(2, 132.5), (1, 121.25), (3, 137.0)], 5: [(1, 40.86), (2, 181.6), (3, 971.33)], 7: [(1, 93.93), (2, 956.8), (3, 18709.8)], 9: [(1, 175.47), (2, 3786.2), (3, 85940.88)]}, 'minimax': {3: [(1, 73.5), (2, 143.25), (3, 223.5)], 5: [(1, 22.43), (2, 249.4), (3, 4194.0)], 7: [(1, 72.4), (2, 1800.07), (3, 82081.6)], 9: [(1, 164.63), (2, 3764.6), (3, 244293.88)]}, 'parallelminimaxab': {3: [(1, 131.67), (2, 115.25), (3, 65.0)], 5: [(1, 46.43), (2, 69.3), (3, 856.9)], 7: [(1, 76.13), (2, 461.39), (3, 8602.2)], 9: [(1, 71.83), (2, 1270.89), (3, 48324.5)]}}
Branching factor map : {3: [4.142857, 5.285714, 5.285714, 4.8333335, 4.0, 4.142857, 4.142857, 4.142857, 4.0], 5: [22.0, 22.0, 22.0, 10.684211, 11.684211, 11.684211, 14.470589, 12.052631, 14.470589], 7: [31.655172, 31.655172, 31.655172, 33.827587, 25.4, 33.827587, 29.724138, 36.13793, 36.13793], 9: [54.567566, 54.567566, 58.771427, 69.10256, 51.82857, 69.10256, 46.435898, 72.645164, 72.645164]}
"""

