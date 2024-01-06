#!/bin/python3

import concurrent.futures, argparse, subprocess
import matplotlib.pyplot as plt

def run_subprocess(dim, c, mcts_sims, args):
    cmd = [
        "dotnet",
        "run",
        "play",
        f"-s1={args.s1}",
        f"-s2={args.s2}",
        f"-depth={args.depth}",
        "-sim",
        f"-exploration={c}",
        f"-mctssim={mcts_sims}",
        "-verbose=false",
        f"-numsim={args.simulations}",
        f"-dimension={dim}"
    ]

    if args.verbose:
        print(" ".join(cmd))

    """
    ===Results===
    Player A : MCTS won 4/5 games. Win rate : 80%. Average move time(ms) : 6273.73
    Player B : MinimaxAlgorithmABPruning won 1/5 games. Win rate : 20%. Average move time(ms) : 3.27
    Toal moves made across 5 games : 130
    """

    result = subprocess.run(cmd, cwd="../../../Quoridor.ConsoleApp/", capture_output=True)

    line_arr = result.stdout.splitlines()

    if args.verbose:
        print()
        print(f"{dim}x{dim} [{args.s1} : {c}, {mcts_sims}] vs {args.s2}:")
        for line in line_arr:
            print(line.decode("utf-8"))

    
    games_won = int(line_arr[1].decode("utf-8").split('/')[0].split()[-1])
    avg_move_time = float(line_arr[1].split()[-1].strip().decode("utf-8"))

    return c, mcts_sims, dim, games_won, avg_move_time


def main(args):
    dims_to_examine = list(map(int, args.dims_to_examine.split(",")))
    iterations_to_examine = [i for i in range(200, 600, 100)]
    exploration_params_to_examine = [round(0.9 + i/10, 1) for i in range(1, 2)]

    if args.verbose:
        print(f"Dimensions to examine : {dims_to_examine}")
        print(f"Iterations to examine : {iterations_to_examine} ")
        print(f"c values to examing   : {exploration_params_to_examine}")

    # { dimension : [(c, num_sim, time, games_won)] }
    dim_result = { i : [] for i in dims_to_examine}

    with concurrent.futures.ProcessPoolExecutor() as executor:
        futures = []
        for dim in dims_to_examine:
            for iteration in iterations_to_examine:
                for c in exploration_params_to_examine:
                    futures.append(executor.submit(run_subprocess, dim, c, iteration, args))

        for future in concurrent.futures.as_completed(futures):
            c, mcts_sims, dim, games_won, avg_move_time = future.result()
            dim_result[dim].append((c, mcts_sims, avg_move_time, games_won))

            print(dim_result)


if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument('--s1', type=str, default="mcts" ,help='Strategy for player 1')
    parser.add_argument("--sim_agent", type=str, default="semirandom", help='simulation agent for mcts')
    parser.add_argument('--depth', type=str, default="1", help='minimax depth')
    parser.add_argument('--s2', type=str, default="minimaxab", help='Strategy for player 2')
    parser.add_argument('--simulations', type=int, default=100, help='Number of simulations')
    parser.add_argument("--verbose", action="store_true", default=False, help='verbose flag. diaplay braching factor, time results')
    parser.add_argument("--dims_to_examine", type=str, default="3,5,7,9", help='dimensions to examine branching factor and times for')
    args = parser.parse_args()
    main(args)