#!/bin/python3

import concurrent.futures, argparse, subprocess
import matplotlib.pyplot as plt

def run_subprocess(dim, s1, s2, args):
    cmd = [
        "dotnet",
        "run",
        "play",
        f"-s1=mcts",
        f"-s2={s2}",
        f"-depth={args.depth}",
        "-sim",
        "-verbose=false",
        f"-numsim={args.simulations}",
        f"-dimension={dim}"
    ]

    if args.verbose:
        print(" ".join(cmd))

    result = subprocess.run(cmd, cwd="../../../Quoridor.ConsoleApp/", capture_output=True)

    line_arr = result.stdout.splitlines()

    if args.verbose:
        print()
        print(f"{dim}x{dim} {s1} vs {s2}:")
        for line in line_arr:
            print(line.decode("utf-8"))
    
    games_won_by_s1 = int(line_arr[1].decode("utf-8").split('/')[0].split()[-1])
    return dim, s1, s2, games_won_by_s1


def main(args):
    dims_to_examine = list(map(int, args.dims_to_examine.split(",")))
    agents = args.agents.split(",")

    if args.verbose:
        print(f"Dimensions to examine : {dims_to_examine}")
        print(f"Agents to examine     : {agents}")

    # { dimension : { agent1 : [(agent2, wins)]} }
    dim_result = { dim : { s1 : [] for s1 in agents } for dim in dims_to_examine }

    with concurrent.futures.ProcessPoolExecutor() as executor:
        futures = []
        for dim in dims_to_examine:
            for s1 in agents:
                futures.append(executor.submit(run_subprocess, dim, "mcts", s1, args))

        for future in concurrent.futures.as_completed(futures):
            dim, f_s, s_s, games_won_by_f_s = future.result()

            dim_result[dim][f_s].append((s_s, games_won_by_f_s))

            print(dim_result)


if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument('--agents', type=str, default="random,semirandom,astar", help='Strategies for tournament')
    parser.add_argument('--depth', type=int, default=1, help='minimax depth')
    parser.add_argument('--simulations', type=int, default=100, help='Number of simulations')
    parser.add_argument("--verbose", action="store_true", default=False, help='verbose flag. diaplay braching factor, time results')
    parser.add_argument("--dims_to_examine", type=str, default="5", help='dimensions to examine branching factor and times for')
    args = parser.parse_args()
    main(args)