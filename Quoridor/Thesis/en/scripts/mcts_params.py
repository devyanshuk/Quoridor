#!/bin/python3

import concurrent.futures, argparse, subprocess
import matplotlib.pyplot as plt

def run_subprocess(c, mcts_sims, args):
    cmd = [
        "dotnet",
        "run",
        "play",
        f"-s1=mcts",
        f"-mctsagent={args.sim_agent}",
        f"-s2={args.s2}",
        f"-depth={args.depth}",
        "-sim",
        f"-exploration={c}",
        f"-mctssim={mcts_sims}",
        "-verbose=false",
        f"-numsim={args.simulations}",
        f"-dimension={args.dim}"
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
        print(f"{args.dim}x{args.dim} [mcts : {c}, {mcts_sims}] vs {args.s2}:")
        for line in line_arr:
            print(line.decode("utf-8"))

    
    games_won = int(line_arr[1].decode("utf-8").split('/')[0].split()[-1])
    avg_move_time = float(line_arr[1].split()[-1].strip().decode("utf-8"))

    return c, mcts_sims, args.dim, games_won, avg_move_time

def draw_sim_time_graph(vals, args):
    """
    [(exploration parameter, mcts simulation, avg time per move, games won / total games)]
    [(1.44, 10, 498.67, 7), (1.44, 30, 2586.59, 37), (1.44, 60, 5150.84, 33), (1.44, 90, 7385.0, 44), (1.44, 120, 10115.54, 39), (1.44, 150, 12678.84, 41), (1.44, 180, 16528.45, 39), (1.44, 210, 17606.21, 42), (1.44, 240, 16972.91, 41)]
    """
    _, simulations, avg_time_per_move, games_won = zip(*vals)

    fig, ax1 = plt.subplots()
    ax1.plot(simulations, games_won, color='tab:blue', label=f'Games won / {args.simulations}')
    ax1.set_xlabel('MCTS simulations')
    ax1.set_ylabel(f'Games won / {args.simulations}', color='tab:blue')
    ax1.tick_params(axis='y', labelcolor='tab:blue')
    ax1.set_xticks(simulations)
    ax1.grid(True)

    ax2 = ax1.twinx()
    ax2.set_xticks(simulations)
    ax2.plot(simulations, avg_time_per_move, color='tab:orange', label='Average time per move (ms)')
    ax2.set_ylabel('Average time per move (ms)', color='tab:orange')
    ax2.tick_params(axis='y', labelcolor='tab:orange')

    fig.tight_layout()
    fig.legend(loc="lower right", bbox_to_anchor=(0.87, 0.14))

    plt.savefig('../../img/mcts_simulation_grid_search.png')
    plt.show()

def draw_exp_param_time_graph(vals, args):
    exploration_parameter, _, avg_time_per_move, games_won = zip(*vals)

    fig, ax1 = plt.subplots()
    ax1.plot(exploration_parameter, games_won, color='tab:blue', label=f'Games won / {args.simulations}')
    ax1.set_xlabel('Exploration parameter')
    ax1.set_ylabel(f'Games won / {args.simulations}', color='tab:blue')
    ax1.tick_params(axis='y', labelcolor='tab:blue')
    ax1.set_xticks(exploration_parameter)
    ax1.grid(True)

    ax2 = ax1.twinx()
    ax2.set_xticks(exploration_parameter)
    ax2.plot(exploration_parameter, avg_time_per_move, color='tab:orange', label='Average time per move (ms)')
    ax2.set_ylabel('Average time per move (ms)', color='tab:orange')
    ax2.tick_params(axis='y', labelcolor='tab:orange')

    fig.tight_layout()
    fig.legend(loc="lower right", bbox_to_anchor=(0.87, 0.14))

    plt.savefig('../../img/mcts_exploration_param_grid_search.png')
    plt.show()

   
def main(args):
    vals_to_examine = [i for i in range(30, 270, 30)] if args.examine_simulations else [round(0.9 + i/10, 1) for i in range(1, 10)]

    if args.verbose:
        print(f"Values to examine : {vals_to_examine} ")

    #[(c, num_sim, time, games_won)]
    dim_result = []

    with concurrent.futures.ProcessPoolExecutor() as executor:
        futures = []
        for val in vals_to_examine:
            curr_iter = val if args.examine_simulations else args.best_mcts_simulation
            curr_c = val if not args.examine_simulations else 1.44 #sqrt(2) is the standard exploration parameter
            
            futures.append(executor.submit(run_subprocess, curr_c, curr_iter, args))

        for future in concurrent.futures.as_completed(futures):
            c, mcts_sims, dim, games_won, avg_move_time = future.result()
            dim_result.append((c, mcts_sims, avg_move_time, games_won))

            print(dim_result)

    if args.examine_simulations:
        draw_sim_time_graph(dim_result, args)
    else:
        draw_exp_param_time_graph(dim_result, args)

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--sim_agent", type=str, default="parallelminimaxab", help='simulation agent for mcts')
    parser.add_argument('--depth', type=int, default=1, help='minimax depth')
    parser.add_argument('--s2', type=str, default="minimaxab", help='Strategy for player 2')
    parser.add_argument('--best_mcts_simulation', type=int, default=100, help='Best simulation from experiment using --examine_simulations')
    parser.add_argument('--simulations', type=int, default=50, help='number of experiments to run for each value')
    parser.add_argument('--examine_simulations', action="store_true", default=True, help='Check win rates and time against different simulations')
    parser.add_argument("--verbose", action="store_true", default=False, help='verbose flag. diaplay braching factor, time results')
    parser.add_argument("--dim", type=int, default=5, help='dimension to examine mcts win rate and times for')
    args = parser.parse_args()
    main(args)